//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace MapGIS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MapGIS.IGISService")]
    public interface IGISService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/ReWirteTxtFile_OZ", ReplyAction="http://tempuri.org/IGISService/ReWirteTxtFile_OZResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.ReWirteTxtFile_OZResponse> ReWirteTxtFile_OZAsync(MapGIS.ReWirteTxtFile_OZRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/TxtToRaster", ReplyAction="http://tempuri.org/IGISService/TxtToRasterResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.TxtToRasterResponse> TxtToRasterAsync(MapGIS.TxtToRasterRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/JsonPointToRaster", ReplyAction="http://tempuri.org/IGISService/JsonPointToRasterResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.JsonPointToRasterResponse> JsonPointToRasterAsync(MapGIS.JsonPointToRasterRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/RasterToLineAndPolygon", ReplyAction="http://tempuri.org/IGISService/RasterToLineAndPolygonResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonResponse> RasterToLineAndPolygonAsync(MapGIS.RasterToLineAndPolygonRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/RasterToLineAndPolygon_GeoJSON", ReplyAction="http://tempuri.org/IGISService/RasterToLineAndPolygon_GeoJSONResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygon_GeoJSONResponse> RasterToLineAndPolygon_GeoJSONAsync(MapGIS.RasterToLineAndPolygon_GeoJSONRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/RasterToLineAndPolygonByZone_GeoJSON", ReplyAction="http://tempuri.org/IGISService/RasterToLineAndPolygonByZone_GeoJSONResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonByZone_GeoJSONResponse> RasterToLineAndPolygonByZone_GeoJSONAsync(MapGIS.RasterToLineAndPolygonByZone_GeoJSONRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/RasterToStatisticsTable", ReplyAction="http://tempuri.org/IGISService/RasterToStatisticsTableResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableResponse> RasterToStatisticsTableAsync(MapGIS.RasterToStatisticsTableRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGISService/RasterToStatisticsTableByReclassConfig", ReplyAction="http://tempuri.org/IGISService/RasterToStatisticsTableByReclassConfigResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableByReclassConfigResponse> RasterToStatisticsTableByReclassConfigAsync(MapGIS.RasterToStatisticsTableByReclassConfigRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReWirteTxtFile_OZ", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ReWirteTxtFile_OZRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTxtcontendOrFile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public double n;
        
        public ReWirteTxtFile_OZRequest()
        {
        }
        
        public ReWirteTxtFile_OZRequest(string strTxtcontendOrFile, double n)
        {
            this.strTxtcontendOrFile = strTxtcontendOrFile;
            this.n = n;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReWirteTxtFile_OZResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ReWirteTxtFile_OZResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ReWirteTxtFile_OZResult;
        
        public ReWirteTxtFile_OZResponse()
        {
        }
        
        public ReWirteTxtFile_OZResponse(string ReWirteTxtFile_OZResult)
        {
            this.ReWirteTxtFile_OZResult = ReWirteTxtFile_OZResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TxtToRaster", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class TxtToRasterRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTxtcontend;
        
        public TxtToRasterRequest()
        {
        }
        
        public TxtToRasterRequest(string strTxtcontend)
        {
            this.strTxtcontend = strTxtcontend;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TxtToRasterResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class TxtToRasterResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string TxtToRasterResult;
        
        public TxtToRasterResponse()
        {
        }
        
        public TxtToRasterResponse(string TxtToRasterResult)
        {
            this.TxtToRasterResult = TxtToRasterResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="JsonPointToRaster", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class JsonPointToRasterRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTxtcontend;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string extendSHPNAME;
        
        public JsonPointToRasterRequest()
        {
        }
        
        public JsonPointToRasterRequest(string strTxtcontend, string extendSHPNAME)
        {
            this.strTxtcontend = strTxtcontend;
            this.extendSHPNAME = extendSHPNAME;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="JsonPointToRasterResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class JsonPointToRasterResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string JsonPointToRasterResult;
        
        public JsonPointToRasterResponse()
        {
        }
        
        public JsonPointToRasterResponse(string JsonPointToRasterResult)
        {
            this.JsonPointToRasterResult = JsonPointToRasterResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygon", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygonRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rasterID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string configFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string exetentFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public int type;
        
        public RasterToLineAndPolygonRequest()
        {
        }
        
        public RasterToLineAndPolygonRequest(string rasterID, string configFileName, string exetentFileName, int type)
        {
            this.rasterID = rasterID;
            this.configFileName = configFileName;
            this.exetentFileName = exetentFileName;
            this.type = type;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygonResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygonResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays", DataType="base64Binary")]
        public byte[][] RasterToLineAndPolygonResult;
        
        public RasterToLineAndPolygonResponse()
        {
        }
        
        public RasterToLineAndPolygonResponse(byte[][] RasterToLineAndPolygonResult)
        {
            this.RasterToLineAndPolygonResult = RasterToLineAndPolygonResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygon_GeoJSON", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygon_GeoJSONRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rasterID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string configFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string exetentFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public int type;
        
        public RasterToLineAndPolygon_GeoJSONRequest()
        {
        }
        
        public RasterToLineAndPolygon_GeoJSONRequest(string rasterID, string configFileName, string exetentFileName, int type)
        {
            this.rasterID = rasterID;
            this.configFileName = configFileName;
            this.exetentFileName = exetentFileName;
            this.type = type;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygon_GeoJSONResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygon_GeoJSONResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] RasterToLineAndPolygon_GeoJSONResult;
        
        public RasterToLineAndPolygon_GeoJSONResponse()
        {
        }
        
        public RasterToLineAndPolygon_GeoJSONResponse(string[] RasterToLineAndPolygon_GeoJSONResult)
        {
            this.RasterToLineAndPolygon_GeoJSONResult = RasterToLineAndPolygon_GeoJSONResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygonByZone_GeoJSON", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygonByZone_GeoJSONRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rasterID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string configFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string exetentFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string zoneFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=4)]
        public int type;
        
        public RasterToLineAndPolygonByZone_GeoJSONRequest()
        {
        }
        
        public RasterToLineAndPolygonByZone_GeoJSONRequest(string rasterID, string configFileName, string exetentFileName, string zoneFileName, int type)
        {
            this.rasterID = rasterID;
            this.configFileName = configFileName;
            this.exetentFileName = exetentFileName;
            this.zoneFileName = zoneFileName;
            this.type = type;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToLineAndPolygonByZone_GeoJSONResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToLineAndPolygonByZone_GeoJSONResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] RasterToLineAndPolygonByZone_GeoJSONResult;
        
        public RasterToLineAndPolygonByZone_GeoJSONResponse()
        {
        }
        
        public RasterToLineAndPolygonByZone_GeoJSONResponse(string[] RasterToLineAndPolygonByZone_GeoJSONResult)
        {
            this.RasterToLineAndPolygonByZone_GeoJSONResult = RasterToLineAndPolygonByZone_GeoJSONResult;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    public partial class RasterToStatisticsTableResponseRasterToStatisticsTableResult
    {
        
        private System.Xml.Linq.XElement[] anyField;
        
        private System.Xml.Linq.XElement any1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace="http://www.w3.org/2001/XMLSchema", Order=0)]
        public System.Xml.Linq.XElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace="urn:schemas-microsoft-com:xml-diffgram-v1", Order=1)]
        public System.Xml.Linq.XElement Any1
        {
            get
            {
                return this.any1Field;
            }
            set
            {
                this.any1Field = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToStatisticsTable", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToStatisticsTableRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rasterID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string zoneShpFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string zoneFieldName;
        
        public RasterToStatisticsTableRequest()
        {
        }
        
        public RasterToStatisticsTableRequest(string rasterID, string zoneShpFileName, string zoneFieldName)
        {
            this.rasterID = rasterID;
            this.zoneShpFileName = zoneShpFileName;
            this.zoneFieldName = zoneFieldName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToStatisticsTableResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToStatisticsTableResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MapGIS.RasterToStatisticsTableResponseRasterToStatisticsTableResult RasterToStatisticsTableResult;
        
        public RasterToStatisticsTableResponse()
        {
        }
        
        public RasterToStatisticsTableResponse(MapGIS.RasterToStatisticsTableResponseRasterToStatisticsTableResult RasterToStatisticsTableResult)
        {
            this.RasterToStatisticsTableResult = RasterToStatisticsTableResult;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    public partial class RasterToStatisticsTableByReclassConfigResponseRasterToStatisticsTableByReclassConfigResult
    {
        
        private System.Xml.Linq.XElement[] anyField;
        
        private System.Xml.Linq.XElement any1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace="http://www.w3.org/2001/XMLSchema", Order=0)]
        public System.Xml.Linq.XElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace="urn:schemas-microsoft-com:xml-diffgram-v1", Order=1)]
        public System.Xml.Linq.XElement Any1
        {
            get
            {
                return this.any1Field;
            }
            set
            {
                this.any1Field = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToStatisticsTableByReclassConfig", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToStatisticsTableByReclassConfigRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string rasterID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string configFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string zoneShpFileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string zoneFieldName;
        
        public RasterToStatisticsTableByReclassConfigRequest()
        {
        }
        
        public RasterToStatisticsTableByReclassConfigRequest(string rasterID, string configFileName, string zoneShpFileName, string zoneFieldName)
        {
            this.rasterID = rasterID;
            this.configFileName = configFileName;
            this.zoneShpFileName = zoneShpFileName;
            this.zoneFieldName = zoneFieldName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RasterToStatisticsTableByReclassConfigResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RasterToStatisticsTableByReclassConfigResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MapGIS.RasterToStatisticsTableByReclassConfigResponseRasterToStatisticsTableByReclassConfigResult RasterToStatisticsTableByReclassConfigResult;
        
        public RasterToStatisticsTableByReclassConfigResponse()
        {
        }
        
        public RasterToStatisticsTableByReclassConfigResponse(MapGIS.RasterToStatisticsTableByReclassConfigResponseRasterToStatisticsTableByReclassConfigResult RasterToStatisticsTableByReclassConfigResult)
        {
            this.RasterToStatisticsTableByReclassConfigResult = RasterToStatisticsTableByReclassConfigResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface IGISServiceChannel : MapGIS.IGISService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class GISServiceClient : System.ServiceModel.ClientBase<MapGIS.IGISService>, MapGIS.IGISService
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public GISServiceClient() : 
                base(GISServiceClient.GetDefaultBinding(), GISServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WSHttpBinding_IGISService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GISServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(GISServiceClient.GetBindingForEndpoint(endpointConfiguration), GISServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GISServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(GISServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GISServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(GISServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GISServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.ReWirteTxtFile_OZResponse> MapGIS.IGISService.ReWirteTxtFile_OZAsync(MapGIS.ReWirteTxtFile_OZRequest request)
        {
            return base.Channel.ReWirteTxtFile_OZAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.ReWirteTxtFile_OZResponse> ReWirteTxtFile_OZAsync(string strTxtcontendOrFile, double n)
        {
            MapGIS.ReWirteTxtFile_OZRequest inValue = new MapGIS.ReWirteTxtFile_OZRequest();
            inValue.strTxtcontendOrFile = strTxtcontendOrFile;
            inValue.n = n;
            return ((MapGIS.IGISService)(this)).ReWirteTxtFile_OZAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.TxtToRasterResponse> MapGIS.IGISService.TxtToRasterAsync(MapGIS.TxtToRasterRequest request)
        {
            return base.Channel.TxtToRasterAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.TxtToRasterResponse> TxtToRasterAsync(string strTxtcontend)
        {
            MapGIS.TxtToRasterRequest inValue = new MapGIS.TxtToRasterRequest();
            inValue.strTxtcontend = strTxtcontend;
            return ((MapGIS.IGISService)(this)).TxtToRasterAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.JsonPointToRasterResponse> MapGIS.IGISService.JsonPointToRasterAsync(MapGIS.JsonPointToRasterRequest request)
        {
            return base.Channel.JsonPointToRasterAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.JsonPointToRasterResponse> JsonPointToRasterAsync(string strTxtcontend, string extendSHPNAME)
        {
            MapGIS.JsonPointToRasterRequest inValue = new MapGIS.JsonPointToRasterRequest();
            inValue.strTxtcontend = strTxtcontend;
            inValue.extendSHPNAME = extendSHPNAME;
            return ((MapGIS.IGISService)(this)).JsonPointToRasterAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonResponse> MapGIS.IGISService.RasterToLineAndPolygonAsync(MapGIS.RasterToLineAndPolygonRequest request)
        {
            return base.Channel.RasterToLineAndPolygonAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonResponse> RasterToLineAndPolygonAsync(string rasterID, string configFileName, string exetentFileName, int type)
        {
            MapGIS.RasterToLineAndPolygonRequest inValue = new MapGIS.RasterToLineAndPolygonRequest();
            inValue.rasterID = rasterID;
            inValue.configFileName = configFileName;
            inValue.exetentFileName = exetentFileName;
            inValue.type = type;
            return ((MapGIS.IGISService)(this)).RasterToLineAndPolygonAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygon_GeoJSONResponse> MapGIS.IGISService.RasterToLineAndPolygon_GeoJSONAsync(MapGIS.RasterToLineAndPolygon_GeoJSONRequest request)
        {
            return base.Channel.RasterToLineAndPolygon_GeoJSONAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygon_GeoJSONResponse> RasterToLineAndPolygon_GeoJSONAsync(string rasterID, string configFileName, string exetentFileName, int type)
        {
            MapGIS.RasterToLineAndPolygon_GeoJSONRequest inValue = new MapGIS.RasterToLineAndPolygon_GeoJSONRequest();
            inValue.rasterID = rasterID;
            inValue.configFileName = configFileName;
            inValue.exetentFileName = exetentFileName;
            inValue.type = type;
            return ((MapGIS.IGISService)(this)).RasterToLineAndPolygon_GeoJSONAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonByZone_GeoJSONResponse> MapGIS.IGISService.RasterToLineAndPolygonByZone_GeoJSONAsync(MapGIS.RasterToLineAndPolygonByZone_GeoJSONRequest request)
        {
            return base.Channel.RasterToLineAndPolygonByZone_GeoJSONAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.RasterToLineAndPolygonByZone_GeoJSONResponse> RasterToLineAndPolygonByZone_GeoJSONAsync(string rasterID, string configFileName, string exetentFileName, string zoneFileName, int type)
        {
            MapGIS.RasterToLineAndPolygonByZone_GeoJSONRequest inValue = new MapGIS.RasterToLineAndPolygonByZone_GeoJSONRequest();
            inValue.rasterID = rasterID;
            inValue.configFileName = configFileName;
            inValue.exetentFileName = exetentFileName;
            inValue.zoneFileName = zoneFileName;
            inValue.type = type;
            return ((MapGIS.IGISService)(this)).RasterToLineAndPolygonByZone_GeoJSONAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableResponse> MapGIS.IGISService.RasterToStatisticsTableAsync(MapGIS.RasterToStatisticsTableRequest request)
        {
            return base.Channel.RasterToStatisticsTableAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableResponse> RasterToStatisticsTableAsync(string rasterID, string zoneShpFileName, string zoneFieldName)
        {
            MapGIS.RasterToStatisticsTableRequest inValue = new MapGIS.RasterToStatisticsTableRequest();
            inValue.rasterID = rasterID;
            inValue.zoneShpFileName = zoneShpFileName;
            inValue.zoneFieldName = zoneFieldName;
            return ((MapGIS.IGISService)(this)).RasterToStatisticsTableAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableByReclassConfigResponse> MapGIS.IGISService.RasterToStatisticsTableByReclassConfigAsync(MapGIS.RasterToStatisticsTableByReclassConfigRequest request)
        {
            return base.Channel.RasterToStatisticsTableByReclassConfigAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapGIS.RasterToStatisticsTableByReclassConfigResponse> RasterToStatisticsTableByReclassConfigAsync(string rasterID, string configFileName, string zoneShpFileName, string zoneFieldName)
        {
            MapGIS.RasterToStatisticsTableByReclassConfigRequest inValue = new MapGIS.RasterToStatisticsTableByReclassConfigRequest();
            inValue.rasterID = rasterID;
            inValue.configFileName = configFileName;
            inValue.zoneShpFileName = zoneShpFileName;
            inValue.zoneFieldName = zoneFieldName;
            return ((MapGIS.IGISService)(this)).RasterToStatisticsTableByReclassConfigAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WSHttpBinding_IGISService))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WSHttpBinding_IGISService))
            {
                return new System.ServiceModel.EndpointAddress(new System.Uri("http://10.4.94.131:2406/GISService.svc"), new System.ServiceModel.DnsEndpointIdentity("localhost"));
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return GISServiceClient.GetBindingForEndpoint(EndpointConfiguration.WSHttpBinding_IGISService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return GISServiceClient.GetEndpointAddress(EndpointConfiguration.WSHttpBinding_IGISService);
        }
        
        public enum EndpointConfiguration
        {
            
            WSHttpBinding_IGISService,
        }
    }
}
