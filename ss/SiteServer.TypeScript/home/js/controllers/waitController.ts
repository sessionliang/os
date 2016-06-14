class WaitController {
    public returnUrl: string;
    public seconds: number = 5;

    init(): void {
        if (this.returnUrl.length > 0) {
            var intID = setInterval(() => {
                this.seconds--;
                if (this.seconds == 0) {
                    window.top.location.href = this.returnUrl;
                    clearInterval(intID);
                }
                else
                    $("#leftSeconds").html(this.seconds + "s...");
            }, 1000);
        }
    }

    constructor() {
        this.returnUrl = HomeUrlUtils.getUrlVar("returnUrl");
    }
}