(window.webpackJsonp=window.webpackJsonp||[]).push([[10],{238:function(e,t,o){"use strict";o.r(t);var a,r=o(3),l=o(2),n=o(41),d=o(46),i=o(22),w=o(5),c=o(15),p=o(25),s=o(16),g=o(20),h=document.createElement("canvas"),u=h.getContext("2d"),f=d.b,S=((a=u.createLinearGradient(0,0,512*f,0)).addColorStop(0,"red"),a.addColorStop(1/6,"orange"),a.addColorStop(2/6,"yellow"),a.addColorStop(.5,"green"),a.addColorStop(4/6,"aqua"),a.addColorStop(5/6,"blue"),a.addColorStop(1,"purple"),a),b=(h.width=8*f,h.height=8*f,u.fillStyle="white",u.fillRect(0,0,h.width,h.height),u.fillStyle="rgba(102, 0, 102, 0.5)",u.beginPath(),u.arc(4*f,4*f,3*f,0,2*Math.PI),u.fill(),u.fillStyle="rgb(55, 0, 170)",u.beginPath(),u.arc(4*f,4*f,1.5*f,0,2*Math.PI),u.fill(),u.createPattern(h,"repeat")),C=new p.a,y=new s.c({fill:C,stroke:new g.a({color:"#333",width:2})}),m=new i.a({source:new c.a({url:"data/geojson/countries.geojson",format:new n.a}),style:function(e){var t=e.getId();return C.setColor("J"<t?S:b),y}});new r.a({layers:[m],target:"map",view:new l.a({center:Object(w.f)([16,48]),zoom:3})})}},[[238,0]]]);
//# sourceMappingURL=canvas-gradient-pattern.js.map