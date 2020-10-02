export async function sendFriendRequest(userName) {
    try {
        const result = await fetch(`/User/Home/SendFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userName),
        });

        return await result.json();
    }
    catch (error) {
        console.log(error);
    }
}

export async function acceptFriendRequest(id) {
    try {
        const result = await fetch(`/User/Home/AcceptFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(id)
        });

        return await result.json();
    }
    catch (error) {
        console.log(error);
    }
}

export async function rejectFriendRequest(id) {
    try {
        const result = await fetch(`/User/Home/RejectFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(id)
        });

        return await result.json();
    }
    catch (error) {
        console.log(error);
    }
}

export async function getMessagesOfCurrentRelationShipAndRelationShipId(friendId) {
    try {
        const result = await fetch(`/User/Home/GetMessagesFromCurrentRelationShipAndRelationShipId`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(friendId)
        });

        return await result.json();
    }
    catch (error) {
        console.log(error);
    }
}

export async function addMessage(messageContent, relationShipId) {
    try {
       await $.ajax({
            url: '/User/Home/AddMessage',
            type: 'POST',
            data: {
                messageContent: messageContent,
                relationShipId: relationShipId
            },
       });
    }
    catch (error) {
        console.log(error);
    }

    //try {
    //    await fetch(`/User/Home/AddMessage`, {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json',
    //        },
    //        data: {
    //            MessageContent: messageContent,
    //            RelationShipId: relationShipId
    //        },
    //    });
    //}
    //catch (error) {
    //    console.log(error);
    //}
}