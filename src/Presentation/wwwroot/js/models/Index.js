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
    catch(error){
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
    catch(error){
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

export async function getMessagesOfCurrentRelationShip(friendId) {
    try {
        const result = await fetch(`/User/Home/GetMessagesByCurrentUserIdAndFriendId`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(friendId)
        });

        return await result.json();
    }
    catch(error){
        console.log(error);
    }
}