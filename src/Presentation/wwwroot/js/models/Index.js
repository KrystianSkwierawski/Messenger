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