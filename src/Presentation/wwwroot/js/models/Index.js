export async function sendFriendRequest(userName) {
    try {
        const result = await $.ajax({
            url: '/User/Home/SendFriendRequest',
            type: 'POST',
            data: {
                userName: userName
            }
        });

        return result;
    }
    catch (error) {
        console.log(error);
    }
}

export async function acceptFriendRequest(id) {
    try {

        const result = await $.ajax({
            url: '/User/Home/AcceptFriendRequest',
            type: 'POST',
            data: {
                invitingUserId: id
            }
        });

        return result;
    }
    catch (error) {
        console.log(error);
    }
}

export async function rejectFriendRequest(id) {
    try {
        const result = await $.ajax({
            url: '/User/Home/RejectFriendRequest',
            type: 'POST',
            data: {
                invitingUserId: id
            }
        });

        return result;
    }
    catch (error) {
        console.log(error);
    }
}

export async function getMessagesOfCurrentRelationShipAndRelationShipId(friendId) {
    try {
        const result = await $.ajax({
            url: '/User/Home/GetMessagesFromCurrentRelationShipAndRelationShipId',
            type: 'GET',
            data: {
                friendId: friendId
            }
        });

        return result;
    }
    catch (error) {
        console.log(error);
    }
}

export async function addMessage(messageContent, relationShipId) {
    try {
       const result = await $.ajax({
            url: '/User/Home/AddMessage',
            type: 'POST',
            data: {
                messageContent: messageContent,
                relationShipId: relationShipId
            },
       });

       return result;
    }
    catch (error) {
        console.log(error);
    }
}

export async function removeMessage(messageId) {
    try {
        await $.ajax({
            url: '/User/Home/RemoveMessage',
            type: 'POST',
            data: {
                messageId: messageId
            },
        });
    }
    catch (error) {
        console.log(error);
    }
}

export async function getFriendsAndRelationShips() {
    try {
        const result = await $.ajax({
            url: '/User/Home/GetFriendsAndRelationShips',
            type: 'GET'
        });

        return result;
    }
    catch (error) {
        console.log(error);
    }
}