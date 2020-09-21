export async function sendFriendRequest(userName) {
    try {
        const result = await fetch(`/User/Home/SendFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userName),
        });

        const data = await result.json();
        return data;
    }
    catch(error){
        console.log(error);
    }
}

export async function acceptFriendRequest(id) {
    try {
        await fetch(`/User/Home/AcceptFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(id)
        });
    }
    catch(error){
        console.log(error);
    }
}

export async function rejectFriendRequest(id) {
    try {
        await fetch(`/User/Home/RejectFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(id)
        });
    }
    catch (error) {
        console.log(error);
    }
}