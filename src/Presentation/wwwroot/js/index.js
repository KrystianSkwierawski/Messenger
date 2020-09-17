import { elements, elementStrings } from './views/base.js';
import * as indexView from './views/taskView.js';
import * as Index from './models/Index.js';

elements.menuButton.addEventListener('click', () => {
    indexView.slideOutSideMenu();
});

elements.addFriendButton.addEventListener('click', addFriend);
   
elements.friendsContainer.addEventListener('click', e => {

    if (e.target.matches(`.${elementStrings.friendAcceptRequest}`)) {
        console.log('accept');
    }

    if (e.target.matches(`.${elementStrings.friendRejectRequest}`)) {
        console.log('reject');
    }
});

async function addFriend(){
    const friendName = prompt('Friend name:');  

    if (ifDoesNotInvitingHimSelf(friendName) && ifDoesNotHaveThisFriend(friendName)) {
        const request = await Index.sendFriendRequest(friendName);
        indexView.setFriendDataset(JSON.stringify(request.friends));

        if (request.userExist) {
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

const ifDoesNotHaveThisFriend = friendName => {
    const friends = indexView.getFriends();

    const result = friends.filter(x => x.userName === friendName);

    return result.length === 0 ? true : false;
};

const ifDoesNotInvitingHimSelf = friendName => {
    const currentUserId = indexView.getUserName();  

    return friendName === currentUserId ? false : true;
}
