/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

//service's base class
class ServiceBase {
    //serviceName  demo : user
    private serviceName: string = StringUtils.empty;

    public set ServiceName(serviceName: string) {
        this.serviceName = serviceName;
    }
    public get ServiceName() {
        return this.serviceName;
    }

    //constructor
    constructor(serviceName: string) {
        this.serviceName = serviceName;
    }

    getUrl(action: string, id?: number) {
        return UrlUtils.getAPI(this.serviceName, action, id);
    }
} 