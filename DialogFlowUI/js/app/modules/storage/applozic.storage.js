var ALStorage = (function (win) {
    var MCK_LATEST_MESSAGE_ARRAY = [];
    var MCK_MESSAGE_ARRAY = [];
    var MCK_MESSAGE_MAP = [];
    var MCK_CONTACT_NAME_ARRAY = [];
    var FRIEND_LIST_GROUP_NAME;
    var FRIEND_LIST_GROUP_TYPE;
    var ENCRYPTION_KEY;

    return {

        setEncryptionKey: function (encryptionKey) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem('encryptionKey', encryptionKey);
            } else {
                ENCRYPTION_KEY = encryptionKey;
            }
        },
        getEncryptionKey: function (encryptionKey) {
            return (ALStorage.isSessionStorageAvailable()) ? w.sessionStorage.getItem("encryptionKey") : ENCRYPTION_KEY;
        },
        removeEncryptionKey: function () {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.removeItem('encryptionKey');
            }
        },
        updateLatestMessage: function (message) {
            var messageArray = [];
            messageArray.push(message);
            ALStorage.updateLatestMessageArray(messageArray);
            ALStorage.updateMckMessageArray(messageArray);
        },
        getLatestMessageArray: function () {
            return (ALStorage.isSessionStorageAvailable()) ? $applozic.parseJSON(w.sessionStorage.getItem("mckLatestMessageArray")) : MCK_LATEST_MESSAGE_ARRAY;
        },
        getFriendListGroupName: function () {
            return (ALStorage.isSessionStorageAvailable()) ? w.sessionStorage.getItem("friendListGroupName") : FRIEND_LIST_GROUP_NAME;
        },
        setFriendListGroupName: function (friendListGroupName) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem('friendListGroupName', friendListGroupName);
            } else {
                FRIEND_LIST_GROUP_NAME = friendListGroupName;
            }
        },
        setFriendListGroupType: function (friendListGroupType) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem('friendListGroupType', friendListGroupType);
            } else {
                FRIEND_LIST_GROUP_TYPE = friendListGroupType;
            }
        },
        getFriendListGroupType: function () {
            return (ALStorage.isSessionStorageAvailable()) ? w.sessionStorage.getItem("friendListGroupType") : FRIEND_LIST_GROUP_TYPE;
        },
        setLatestMessageArray: function (messages) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem('mckLatestMessageArray', w.JSON.stringify(messages));
            } else {
                MCK_LATEST_MESSAGE_ARRAY = messages;
            }
        },
        updateLatestMessageArray: function (mckMessageArray) {
            if (ALStorage.isSessionStorageAvailable()) {
                var mckLocalMessageArray = $applozic.parseJSON(w.sessionStorage.getItem('mckLatestMessageArray'));
                if (mckLocalMessageArray !== null) {
                    mckLocalMessageArray = mckLocalMessageArray.concat(mckMessageArray);
                    w.sessionStorage.setItem('mckLatestMessageArray', w.JSON.stringify(mckLocalMessageArray));
                } else {
                    w.sessionStorage.setItem('mckLatestMessageArray', w.JSON.stringify(mckMessageArray));
                }
                return mckMessageArray;
            } else {
                MCK_LATEST_MESSAGE_ARRAY = MCK_LATEST_MESSAGE_ARRAY.concat(mckMessageArray);
                return MCK_LATEST_MESSAGE_ARRAY;
            }
        },
        getMckMessageArray: function () {
            return (ALStorage.isSessionStorageAvailable()) ? $applozic.parseJSON(w.sessionStorage.getItem("mckMessageArray")) : MCK_MESSAGE_ARRAY;
        },
        clearMckMessageArray: function () {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.removeItem('mckMessageArray');
                w.sessionStorage.removeItem('mckLatestMessageArray');
            } else {
                MCK_MESSAGE_ARRAY.length = 0;
                MCK_LATEST_MESSAGE_ARRAY.length = 0;
            }
        },
        clearAppHeaders: function () {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.removeItem('mckAppHeaders');
            }
        },
        setAppHeaders: function (data) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem("mckAppHeaders", w.JSON.stringify(data));
            }
        },
        getAppHeaders: function (data) {
            return (ALStorage.isSessionStorageAvailable()) ? $applozic.parseJSON(w.sessionStorage.getItem('mckAppHeaders')) : {};
        },
        getMessageByKey: function (key) {
            return MCK_MESSAGE_MAP[key];
        },
        updateMckMessageArray: function (mckMessageArray) {
            for (var i = 0; i < mckMessageArray.length; i++) {
                var message = mckMessageArray[i];
                MCK_MESSAGE_MAP[message.key] = message;
            }
            if (ALStorage.isSessionStorageAvailable()) {
                var mckLocalMessageArray = $applozic.parseJSON(w.sessionStorage.getItem('mckMessageArray'));
                if (mckLocalMessageArray !== null) {
                    mckLocalMessageArray = mckLocalMessageArray.concat(mckMessageArray);
                    w.sessionStorage.setItem('mckMessageArray', w.JSON.stringify(mckLocalMessageArray));
                } else {
                    w.sessionStorage.setItem('mckMessageArray', w.JSON.stringify(mckMessageArray));
                }
                return mckMessageArray;
            } else {
                MCK_MESSAGE_ARRAY = MCK_MESSAGE_ARRAY.concat(mckMessageArray);
                return MCK_MESSAGE_ARRAY;
            }
        },
        getMckContactNameArray: function () {
            return (ALStorage.isSessionStorageAvailable()) ? $applozic.parseJSON(w.sessionStorage.getItem("mckContactNameArray")) : MCK_CONTACT_NAME_ARRAY;
        },
        setMckContactNameArray: function (mckContactNameArray) {
            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.setItem('mckContactNameArray', w.JSON.stringify(mckContactNameArray));
            } else {
                MCK_CONTACT_NAME_ARRAY = mckContactNameArray;
            }
        },
        updateMckContactNameArray: function (mckContactNameArray) {
            if (ALStorage.isSessionStorageAvailable()) {
                var mckLocalcontactNameArray = $applozic.parseJSON(w.sessionStorage.getItem('mckContactNameArray'));
                if (mckLocalcontactNameArray !== null) {
                    mckContactNameArray = mckContactNameArray.concat(mckLocalcontactNameArray);
                }
                w.sessionStorage.setItem('mckContactNameArray', w.JSON.stringify(mckContactNameArray));
                return mckContactNameArray;
            } else {
                MCK_CONTACT_NAME_ARRAY = MCK_CONTACT_NAME_ARRAY.concat(mckContactNameArray);
                return MCK_CONTACT_NAME_ARRAY;
            }
        },
        clearMckContactNameArray: function () {

            if (ALStorage.isSessionStorageAvailable()) {
                w.sessionStorage.removeItem('mckContactNameArray');
            } else {
                MCK_CONTACT_NAME_ARRAY.length = 0;
            }
        },
        clearSessionStorageElements: function () {
            ALStorage.clearMckMessageArray();
            ALStorage.clearAppHeaders();
            ALStorage.clearMckContactNameArray();
            ALStorage.removeEncryptionKey();
        },
        isSessionStorageAvailable: function () {
            try {
                return typeof (w.sessionStorage) !== 'undefined';
            } catch (error) {
                return false;
            }
        }
    };
})(window);
