export async function addFriend(userName) {
    try {
        await fetch(`/User/Home/SendFriendRequest`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userName),
        });
    }
    catch(error){
        console.log(error);
    }
}