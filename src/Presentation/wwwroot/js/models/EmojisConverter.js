const _emojis = new Map();

_emojis.set(":)", "🙂");
_emojis.set(":(", "😟");
_emojis.set("<3", "❤");
_emojis.set(":d", "😀");
_emojis.set(":D", "😀");
_emojis.set(":p", "😛");
_emojis.set(":P", "😛");
_emojis.set(";(", "😭");
_emojis.set(";)", "😉");
_emojis.set(":o", "😮");
_emojis.set(":O", "😮");
_emojis.set(":*", "😗");
_emojis.set(";*", "😘");
_emojis.set(":/", "😕");
_emojis.set(":|", "😐");
_emojis.set("B)", "😎");

export const convertTextToEmojis = (text) => {
    _emojis.forEach((value, key) => {
        text = text.replace(key, value);
    });

    return text;
};