(window.webpackJsonp=window.webpackJsonp||[]).push([[47],{401:function(e,t,r){"use strict";r.r(t);var _=r(14),a=r(185),n=r(115),o=r(13),y=r(0),s=function(r){function e(e){r.call(this);var t=e||{};this.featureNS_="http://mapserver.gis.umn.edu/mapserver",this.gmlFormat_=new a.a,this.layers_=t.layers?t.layers:null}return r&&(e.__proto__=r),((e.prototype=Object.create(r&&r.prototype)).constructor=e).prototype.getLayers=function(){return this.layers_},e.prototype.setLayers=function(e){this.layers_=e},e.prototype.readFeatures_=function(e,t){var r=this;e.setAttribute("namespaceURI",this.featureNS_);var a=e.localName,n=[];if(0===e.childNodes.length)return n;if("msGMLOutput"==a)for(var o=0,s=e.childNodes.length;o<s;o++){var u=e.childNodes[o];if(u.nodeType===Node.ELEMENT_NODE){var l=u,i=t[0],c=l.localName.replace("_layer","");if(!r.layers_||Object(_.f)(r.layers_,c)){var m=c+"_feature";i.featureType=m,i.featureNS=r.featureNS_;var h={};h[m]=Object(y.i)(r.gmlFormat_.readFeatureElement,r.gmlFormat_);var p=Object(y.q)([i.featureNS,null],h);l.setAttribute("namespaceURI",r.featureNS_);var d=Object(y.t)([],p,l,t,r.gmlFormat_);d&&Object(_.c)(n,d)}}}if("FeatureCollection"==a){var f=Object(y.t)([],this.gmlFormat_.FEATURE_COLLECTION_PARSERS,e,[{}],this.gmlFormat_);f&&(n=f)}return n},e.prototype.readFeaturesFromNode=function(e,t){var r={};return t&&Object(o.a)(r,this.getReadOptions(e,t)),this.readFeatures_(e,[r])},e}(n.a);fetch("data/wmsgetfeatureinfo/osm-restaurant-hotel.xml").then(function(e){return e.text()}).then(function(e){var t=(new s).readFeatures(e);document.getElementById("all").innerText=t.length.toString();var r=new s({layers:["hotel"]}).readFeatures(e);document.getElementById("hotel").innerText=r.length.toString();var a=new s({layers:["restaurant"]}).readFeatures(e);document.getElementById("restaurant").innerText=a.length.toString()})}},[[401,0]]]);
//# sourceMappingURL=getfeatureinfo-layers.js.map