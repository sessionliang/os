/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

class UserService {

    private getUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('user', action, id);
    }
    private getOrderUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('order', action, id);
    }

    private getB2CUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('b2c', action, id);
    }

    private getFollowUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('follow', action, id);
    }

    getHomeUrl(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetHomeUrl'), done);
    }

    getUser(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetUser'), done);
    }

    //getUserSync(done: (data) => void): void {
    //    Utils.ajaxGetSync({}, this.getUrl('GetUser'), done);
    //}

    IsPersistent(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('IsPersistent'), done);
    }

    register(userName: string, email: string, password: string, validCode: string, returnUrl: string, done: (data) => void): void {
        var parameters = { loginName: userName, password: password, email: email, mobile: "", validCode: validCode, returnUrl: returnUrl };
        Utils.ajaxGet(parameters, this.getUrl('RegisterWithValidCode'), done);
    }

    login(userName: string, password: string, isPersistent: string, useBase64: boolean, done: (data) => void): void {
        var parameters = { loginName: userName, password: password, isPersistent: isPersistent, useBase64: useBase64 };
        Utils.ajaxGet(parameters, this.getUrl('Login'), done);
    }

    logout(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('Logout'),(data) => {
            Utils.setLoginStatus(false);
            done(data);
        });
    }

    updateDetailUserInfo(bloodType: string, height: number, maritalStatus: string,
        education: string, graduateInstitutions: string, provinceValue: string, address: string, QQ: string, WeiBo: string,
        WeiXin, gender, organization, department, position, interects, graduation, done: (data) => void): void {
        var parameters = {
            bloodType: bloodType, height: height, maritalStatus: maritalStatus, education: education
            , graduateInstitutions: graduateInstitutions, provinceValue: provinceValue, address: address
            , QQ: QQ, WeiBo: WeiBo
            , WeiXin: WeiXin, gender: gender
            , organization: organization, department: department, position: position, interects: interects
            , graduation: graduation
        };
        Utils.ajaxGet(parameters, this.getUrl('UpdateDetailUserInfo'), done);
    }

    updateAutoDetailUserInfo(form: string, done: (data) => void): void {
        var parameters = {
            form: form
        };
        Utils.ajaxPost(parameters, this.getUrl('updateAutoDetailUserInfo'), done);
    }

    updateBasicUserInfo(userName: string, signature: string, done: (data) => void): void {

        var parameters = { userName: userName, signature: signature };
        Utils.ajaxGet(parameters, this.getUrl('UpdateBasicUserInfo'), done);
    }

    changePassword(currentPassword: string, newPassword: string, done: (data) => void): void {

        var parameters = { currentPassword: currentPassword, newPassword: newPassword };
        Utils.ajaxGet(parameters, this.getUrl('ChangePassword'), done);
    }

    getUploadImgUrl(action: string): string {
        return this.getUrl(action);
    }

    getUserMessage(type: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { type: type, pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, this.getUrl('GetUserMessage'), done);
    }

    getUserAllMessage(type: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { type: type, pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, this.getUrl('GetUserAllMessage'), done);
    }


    getUserMessageDetail(messageID: number, done: (data) => void): void {
        var parameters = { messageID: messageID };
        Utils.ajaxGet(parameters, this.getUrl('GetUserMessageDetail'), done);
    }

    deleteUserMessage(messageID: number, done: (data) => void): void {
        var parameters = { messageID: messageID };
        Utils.ajaxGet(parameters, this.getUrl('DeleteUserMessage'), done);
    }

    getSiteMessage(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, this.getUrl('GetSiteMessage'), done);
    }

    sendMessage(userName: string, title: string, msg: string, done: (data) => void, parentID?: number): void {
        var parameters = { userName: userName, title: title, msg: msg, parentID: parentID };
        Utils.ajaxGet(parameters, this.getUrl('SendMessage'), done);
    }

    getUserLoginLog(pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetUserLoginLog'), done);
    }

    getSecurityQuestionList(done: (data) => void, userName?: string): void {
        Utils.ajaxGet({ userName: userName }, this.getUrl('GetSecurityQuestionList'), done);
    }

    updateSecurityQuestion(que1: string, que2: string, que3: string, anw1: string, anw2: string, anw3: string, done: (data) => void): void {
        var parameters = { que1: que1, que2: que2, que3: que3, anw1: anw1, anw2: anw2, anw3: anw3 };
        Utils.ajaxGet(parameters, this.getUrl('UpdateSecurityQuestion'), done);
    }

    validateSecurityQuestion(que: string, anw: string, done: (data) => void): void {
        var parameters = { que: que, anw: anw };
        Utils.ajaxGet(parameters, this.getUrl('ValidateSecurityQuestion'), done);
    }

    getUserSecurityQuestionAnwser(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetUserSecurityQuestionAnwser'), done);
    }

    bindEmail(email: string, done: (data) => void): void {
        var parameters = { email: email };
        Utils.ajaxPost(parameters, this.getUrl('BindEmail'), done);
    }

    bindEmailValidate(email: string, validateCode: string, done: (data) => void): void {
        var parameters = { email: email, validateCode: validateCode };
        Utils.ajaxPost(parameters, this.getUrl('BindEmailValidate'), done);
    }

    findPasswordByEmailStep1(userName: string, done: (data) => void, validateCode?: string): void {
        var parameters = { userName: userName, groupSN: "", validateCode: validateCode };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByEmailStep1'), done);
    }

    findPasswordByEmailStep2(userName: string, email: string, key: string, done: (data) => void): void {
        var parameters = { userName: userName, groupSN: "", email: email, key: key };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByEmailStep2'), done);
    }

    findPasswordByEmailStep3(userName: string, validateCode: string, email: string, key: string, done: (data) => void): void {
        var parameters = { userName: userName, groupSN: "", validateCode: validateCode, email: email, key: key };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByEmailStep3'), done);
    }

    findPasswordByPhoneStep1(userName: string, done: (data) => void, validateCode?: string): void {
        var parameters = { userName: userName, groupSN: "", validateCode: validateCode };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByPhoneStep1'), done);
    }

    findPasswordByPhoneStep2(userName: string, phone: string, key: string, done: (data) => void): void {
        var parameters = { userName: userName, groupSN: "", phone: phone, key: key };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByPhoneStep2'), done);
    }

    findPasswordByPhoneStep3(userName: string, validateCode: string, phone: string, key: string, done: (data) => void): void {
        var parameters = { userName: userName, groupSN: "", validateCode: validateCode, phone: phone, key: key };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordByPhoneStep3'), done);
    }

    findPassword(newPassword: string, userName: string, email: string, type: string, key: string, done: (data) => void): void {
        var parameters = { newPassword: newPassword, userName: userName, groupSN: "", email: email, type: type, key: key };
        Utils.ajaxPost(parameters, this.getUrl('FindPassword'), done);
    }

    findPasswordBySCQUStep1(userName: string, done: (data) => void, validateCode?: string): void {
        var parameters = { userName: userName, groupSN: "", validateCode: validateCode };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordBySCQUStep1'), done);
    }

    findPasswordBySCQUStep2(userName: string, KeyStep1: string, Que: string, Anw: string, done: (data) => void): void {
        var parameters = { userName: userName, groupSN: "", KeyStep1: KeyStep1, Que: Que, Anw: Anw };
        Utils.ajaxPost(parameters, this.getUrl('FindPasswordBySCQUStep2'), done);
    }

    accountSafeLevel(done: (data) => void): void {
        Utils.ajaxPost({}, this.getUrl('AccountSafeLevel'), done);
    }

    sdkLogin(sdkType: string, returnUrl: string, done: (data) => void): void {
        var parameters = { sdkType: sdkType, returnUrl: returnUrl };
        Utils.ajaxGet(parameters, this.getUrl('SdkLogin'), done);
    }

    sdkBind(sdkType: string, returnUrl: string, done: (data) => void): void {
        var parameters = { sdkType: sdkType, returnUrl: returnUrl };
        Utils.ajaxGet(parameters, this.getUrl('SdkBind'), done);
    }


    sdkUnBind(sdkType: string, done: (data) => void): void {
        var parameters = { sdkType: sdkType };
        Utils.ajaxGet(parameters, this.getUrl('SdkUnBind'), done);
    }

    getThirdLoginTypeParameter(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxGet(parameters, this.getUrl('GetThirdLoginTypeParameter'), done);
    }

    getEnablePathList(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxGet(parameters, this.getUrl("getEnablePathList"), done);
    }

    getEnablePathListForMessage(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxGet(parameters, this.getUrl("getEnablePathListForMessage"), done);
    }

    bindPhone(phoneNum: string, done: (data) => void): void {
        var parameters = { phoneNum: phoneNum };
        Utils.ajaxPost(parameters, this.getUrl("bindPhone"), done);
    }

    bindPhoneValidate(phoneNum: string, validateCode: string, done: (data) => void): void {
        var parameters = { phoneNum: phoneNum, validateCode: validateCode };
        Utils.ajaxPost(parameters, this.getUrl("bindPhoneValidate"), done);
    }

    removeBindPhone(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxPost(parameters, this.getUrl("removeBindPhone"), done);
    }

    loadUserProperty(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxGet(parameters, this.getUrl("loadUserProperty"), done);
    }

    getUserInvoices(done: (data) => void): void {
        var parameters = {};
        Utils.ajaxGet(parameters, this.getUrl('GetUserInvoices'), done);
    }

    getUserInvoicesOne(id: number, done: (data) => void): void {
        var parameters = { id: id };
        Utils.ajaxGet(parameters, this.getUrl('getUserInvoicesOne', id), done);
    }

    updateUserInvoice(invoice, done: (data) => void): void {
        Utils.ajaxGet(invoice, this.getOrderUrl('UpdateInvoice'), done);
    }
    addUserInvoice(invoice, done: (data) => void): void {
        Utils.ajaxGet(invoice, this.getOrderUrl('AddInvoice'), done);
    }

    delUserInvoice(id: number, done: (data) => void): void {

        Utils.ajaxGet({}, this.getOrderUrl('DeleteInvoice', id), done);

    }

    getUserShipmentOne(id: number, done: (data) => void): void {
        var parameters = { id: id };
        Utils.ajaxGet(parameters, this.getUrl('getUserShipmentOne', id), done);
    }

    getUserFollows(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, this.getUrl('GetFollowList'), done);
    }

    removeUserFollows(ids: string, done: (data) => void): void {
        var parameters = { 'ids': ids };
        Utils.ajaxGet(parameters, this.getUrl('RemoveUserFollow'), done);
    }

    addToCart(cart, done: (data) => void): void {

        Utils.ajaxGet(cart, this.getB2CUrl('AddToCart'), done);

    }


    followAddToCart(ids: string, done: (data) => void): void {

        Utils.ajaxGet({ 'ids': ids }, this.getUrl('FollowAddToCart'), done);

    }
    getUserCart(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetUserCart'), done);
    }

    isUserSeller(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('IsSeller'), done);
    }

    getUserHistory(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, this.getUrl('GetHistoryList'), done);
    }

    removeUserHistory(ids: string, done: (data) => void): void {
        var parameters = { 'ids': ids };
        Utils.ajaxGet(parameters, this.getUrl('RemoveUserHistory'), done);
    }
    historyAddToCart(ids: string, done: (data) => void): void {

        Utils.ajaxGet({ 'ids': ids }, this.getUrl('HistoryAddToCart'), done);

    }
    getUserAddress(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetConsigneeList'), done);
    }
    getUserAddressOne(id: number, done: (data) => void): void {
        Utils.ajaxGet({ id: id }, this.getUrl('GetConsigneeOne', id), done);
    }
    addUserAddress(address: any, done: (data) => void): void {
        Utils.ajaxGet(address, this.getUrl('AddConsignee'), done);
    }

    updateUserAddress(address: any, done: (data) => void): void {
        Utils.ajaxGet(address, this.getUrl('UpdateConsignee'), done);
    }
    deleteUserAddress(id: number, done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('DeleteConsignee', id), done);
    }
    private getExtUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('ext', action, id);
    }

    getUserGuesses(done: (data) => void): void {
        Utils.ajaxGet({}, this.getExtUrl('GetGuesses'), done);
    }
    deleteCart(cartID: number, publishmentSystemId: number, done: (data) => void) {
        $.getJSON(UrlUtils.getAPI('b2c', 'DeleteCart', cartID) + "?publishmentSystemId=" + publishmentSystemId.toString(), null, done);
    }
}