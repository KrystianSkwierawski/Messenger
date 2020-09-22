import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/taskView.js';
import * as Index from './models/Index.js';

elements.menuButton.addEventListener('click', () => {
    indexView.slideOutSideMenu();
});

elements.addFriendButton.addEventListener('click', trySendFriendRequest);
   
elements.friendsContainer.addEventListener('click', async e => {

    if (e.target.matches(`.${elementStrings.friendAcceptRequest}`)) {
        const friendContainer = e.target.parentNode.parentNode;        
        indexView.removeFriendRequestContainer(friendContainer);

        const relationShips = await Index.acceptFriendRequest(friendContainer.id);

        indexView.setRelationShipsDataset(JSON.stringify(relationShips));
    }

    if (e.target.matches(`.${elementStrings.friendRejectRequest}`)) {
        const friendContainer = e.target.parentNode.parentNode;
        indexView.removeFriendContainer(friendContainer);

        const result = await Index.rejectFriendRequest(friendContainer.id);

        indexView.setFriendDataset(JSON.stringify(result.friends));
        indexView.setRelationShipsDataset(JSON.stringify(result.relationShips));
    }     
});

async function trySendFriendRequest(){
    const friendName = prompt('Friend name:');  

    if (doesNotInvitingHimSelf(friendName) && doesNotHaveThisFriend(friendName)) {
        const request = await Index.sendFriendRequest(friendName);
             
        if (request.userExist) {
            
            indexView.setFriendDataset(JSON.stringify(request.friends));
            toastr.success('The friend request has been sent');
        }
        else {
            toastr.info('There is no such user');
        }
    }
    else {
        console.log('error');
    }
}

elements.searchInput.addEventListener('change', () => {
    const friends = indexView.getFriends();   
    const userName = indexView.getSearchingUserName()

    const filteredFriends = friends.filter(x => x.userName.includes(userName));

    indexView.clearFriendsContainer();
    indexView.renderFriends(filteredFriends);
    
});

const doesNotHaveThisFriend = friendName => {
    const friends = indexView.getFriends();

    const result = friends.filter(x => x.userName === friendName);

    return result.length === 0 ? true : false;
};

const doesNotInvitingHimSelf = friendName => {
    const currentUserId = indexView.getUserName();  

    return friendName === currentUserId ? false : true;
}
