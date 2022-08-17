mergeInto(LibraryManager.library, {
    CheckPlatform: function () { 
        //ユーザーエージェントを取得して全て小文字に変換する
        var ua = window.navigator.userAgent.toLowerCase(); 
        //ユーザーエージェント文字列にandroidかiosが含まれているか
        if(ua.indexOf("android") !== -1 || ua.indexOf("ios") !== -1){
            //今開いているシーンにあるGameManagerというオブジェクトにアタッチされているスクリプトのsetSmartPhoneModeというメソッドを呼ぶ
            unityInstance.SendMessage('KeyboardManager', 'setSmartPhoneMode')
        }
    },
});