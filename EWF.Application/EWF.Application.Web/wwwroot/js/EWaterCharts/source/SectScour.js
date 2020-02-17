/**
**断面套汇图
**Add by SUN
**Date:2019-08-23 15:00
*/
(function ($$) {
	var _self;
    $$.fn.DrawSumSect=function(options){
		_self=this;
		var defaults={
			chart:{
				marginLeft:80,
				height:400,
				events:{
					click: function(e) {
						_self.GC=e.yAxis[0].value.toFixed(2);
						_self.startX=e.xAxis[0].value.toFixed(2);
						ChangeV(_self.startX,_self.GC);
					},
					redraw:function(){
						ShowTable(_self.startX,_self.GC);
					}
				}
			},
			credits:{
				enabled:false
			},
			vLabel:{
				marginLeft:10,
				marginTop:40
			},
			plotOptions:{ 
				series: {
					marker:{
						enabled:false,
						radius:0,
						states:{
							hover:{
								enabled:false
							}
						}
					},
					events: {
						click: function(e) {
							_self.GC=this.yAxis.toValue(e.chartY).toFixed(2);
							_self.startX=this.xAxis.toValue(e.chartX).toFixed(2);
							ChangeV(_self.startX,_self.GC);
						}
				   }
				}
			}
		};
		var settings = $.extend(true, {}, defaults, options);
		
		if(settings.vLabel.marginLeft){
			settings.chart.marginLeft+=settings.vLabel.marginLeft+60;
		}
		
		var chart=Highcharts.chart(this.attr("ID"), settings);
		
		var labelElement=chart.renderer.label(GetLabel(),  settings.vLabel.marginLeft, settings.vLabel.marginTop, 'callout',100,200,true).css({color: '#000000',align: 'left'}).add();
		
		var marginRight=chart.marginRight||chart.margin[1];
		var marginLeft=chart.marginLeft||chart.margin[3]||chart.spacing[3];
		
		var cssdiv="style='margin-left:"+marginLeft+"px;margin-right:"+marginRight+"px;'";
		var divID='div_' +this.attr("ID")+Math.random();
		$(this).append("<div id='"+divID+"' "+cssdiv +"></div>");
		_self.data=chart.series;
		_self.tableID=divID;	
		_self.labelElement=labelElement;
		
		ShowTable();		
	}
	function ChangeV(x,y){		 
		if(_self.labelElement){
			_self.labelElement.attr({text:GetLabel(x,y)});
		}
		ShowTable(x,y);
	}
	function ShowTable(b_x,beginLimitGC){
		
		var table="<table border=1 cellpadding=5 cellspacing=0 width='100%' style='border-color:#ccc'>"
				 +"<tr style='background-color:#eeeeee;'><th>时间</th><th>高程(米)</th><th>面积(平方米)</th><th>左起点距(米)</th><th>右起点距(米)</th><th>平均水深(米)</th><th>水面宽度(米)</th></tr>";
		if(beginLimitGC){
			
			ClearFillArea(); 
			
			for(var i=0;i<_self.data.length;i++){
				
				var chartData=_self.data[i];
				
				if(!chartData.visible){
					continue;
				}
				
				var boundaryIndex=GetStartEndIndex(chartData,b_x,beginLimitGC);
				var begX=boundaryIndex.StartIndex;
				var endX=boundaryIndex.EndIndex;
				var pathData=FillArea(begX,endX,chartData,beginLimitGC);
				var objTr=CalSection(chartData.xData,chartData.yData,beginLimitGC,begX,endX);
				if(objTr){
					table+="<tr><td style='text-align:center;'>"+chartData.name+"</td><td style='text-align:right;'>"+beginLimitGC+"</td><td style='text-align:right;'>"+objTr.AREA+"</td><td style='text-align:right;'>"
						 +chartData.xData[begX]+"</td><td style='text-align:right;'>"+chartData.xData[endX]+"</td><td style='text-align:right;'>"+objTr.DP+"</td><td style='text-align:right;'>"+objTr.SW+"</td></tr>";
				}
				var color=chartData.color;
				chartData.chart.renderer.path(pathData).attr({ fill: color, opacity: .5, zIndex: 2, id: 'myPath'+i }).add();				
			}			 
		}
		table+="</table>";
		document.getElementById(_self.tableID).innerHTML=table;
	}
	function ClearFillArea(){
		for(var nn=0;nn<_self.data.length;nn++){
			$("#myPath"+nn).remove(); 
		}
	}
	function GetStartEndIndex(chartData,b_x,beginLimitGC){
		var begX=0,endX=0;
		var lastIndex=utils.getNearIndex(chartData.xData,b_x);
				
		var nearPY=chartData.yData[lastIndex];
		
		if(beginLimitGC>nearPY){
			var c=0;
			for(var j=lastIndex;j>=0;j--){
				if(chartData.yData[j]>beginLimitGC){
					begX=j;
					c++;
					break;
				}
			}
			if (begX==0&&c==0)
			{
				for (var b1=lastIndex; b1<chartData.yData.length; b1++)
				{
					if (chartData.yData[b1]>beginLimitGC)
					{
						begX = b1;
						break;
					}
				}
			}
			for(var en=lastIndex;en<chartData.yData.length;en++){
				if(chartData.yData[en]>=beginLimitGC){
					endX=en;
					break;
				}
			}
			
		}else{
			for(var k=lastIndex;k<chartData.yData.length;k++){
				if(chartData.yData[k]<=beginLimitGC){
					begX=k;
					break;
				}
			}
			if(begX==0){
				for(var jj=lastIndex;jj>=0;jj--){
					if(chartData.yData[jj]<beginLimitGC){
						begX=jj;
						break;
					}		
				}
			}
			for(var m=lastIndex;m>=0;m--){
				if(chartData.yData[m]<beginLimitGC){
					endX=m;
					break;
				}
			}
			if(endX==0){
				for(var b=lastIndex;b<chartData.yData.length;b++){
					if(chartData.yData[b]<beginLimitGC){
						endX=b;
						break;
					}
				}
			}
		}
		return ({StartIndex:begX,EndIndex:endX});
	}
	function GetLabel(x,y){
		if(!x){
			x="";
		}
		if(!y){
			y="";
		}
		var html="高程<br/><input style='width:80px' value='"+y+"'/><br/>左起点距<br/><input style='width:80px' value='"+x+"'/><br/>右起点距<br/><input style='width:80px' value='"+x+"'/>";
		return html;
	}
	function FillArea(startIndex,endIndex,chartData,limitGC){ 
		var x_axis=chartData.xAxis;
		var yAxisQ=chartData.yAxis;
		var path = [];
		path.push('M');
		var ydata=chartData.yData[startIndex]; 
		var startV=(chartData.xData[startIndex+1]-(chartData.yData[startIndex+1]-limitGC)*(chartData.xData[startIndex+1]-chartData.xData[startIndex])/(chartData.yData[startIndex+1]-chartData.yData[startIndex]));
		if(startV<0){
			startV=0;
		}
		path.push(x_axis.toPixels(startV), yAxisQ.toPixels(limitGC));
		path.push('L');
		for (var k = startIndex+1; k < endIndex; k++) {
			ydata=chartData.yData[k];
			if(ydata>limitGC){
				ydata=limitGC;
			}
			path.push(x_axis.toPixels(chartData.xData[k]), yAxisQ.toPixels(ydata));
		}
		var endV=(chartData.xData[endIndex-1]+(limitGC-chartData.yData[endIndex-1])*(chartData.xData[endIndex]-chartData.xData[endIndex-1])/(chartData.yData[endIndex]-chartData.yData[endIndex-1]));
		path.push(x_axis.toPixels(endV), yAxisQ.toPixels(limitGC));
		return path;
	}
	function CalSection(xData,yData,limitGC,begX,endX){
		if(begX>=endX){
			return null;
		}
		var objDou={
			AREA:0,
			DP:0,
			SW:0
		};		 
		if(limitGC>yData[begX]){
			objDou.AREA+=(limitGC-yData[begX+1])*(xData[begX+1]-xData[begX])/2;
			objDou.DP+=yData[begX];
			objDou.SW+=xData[begX+1]-xData[begX];
		}
		if(objDou.AREA<0){
			objDou.AREA=0;
		}
		var endAREA=0;
		if(limitGC>yData[endX]){
			endAREA=(limitGC-yData[endX-1])*(xData[endX]-xData[endX-1])/2;
			objDou.DP+=yData[endX];
			objDou.SW+=xData[endX]-xData[endX-1];
		}
		if(endAREA>0){
			objDou.AREA+=endAREA;
		}
		var yv1,yv2,n=2;
		for(var i=1;i<endX-begX-1;i++){
			if(yData[begX+i]>limitGC){
				yv1=limitGC;
			}else{
				yv1=yData[begX+i];
				objDou.DP+=yData[begX+i];
				objDou.SW+=xData[begX+i+1]-xData[begX+i];
				n++;
			}
			if(yData[begX+i+1]>limitGC){
				yv2=limitGC;
			}else if(yData[begX+i+i]==0){
				yv2=limitGC;
			}else{
				yv2=yData[begX+i];
			}
			var oneSect=(2*limitGC-yv1-yv2)*(xData[begX+i+1]-xData[begX+i])/2;
			if(oneSect<0){
				oneSect=0;
			}
			objDou.AREA+=oneSect;
		}
		if(objDou.DP>0){
			objDou.DP=objDou.DP/n;
		}
		objDou.DP=objDou.DP.toFixed(2);
		objDou.AREA=objDou.AREA.toFixed(2);
		return objDou;
	}
})(jQuery || window); 