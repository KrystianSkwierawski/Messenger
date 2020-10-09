export const emotes = new Map();

emotes.set(":)", "🙂");
emotes.set(":(", "😟");
emotes.set("<3", "❤");
emotes.set(":d", "😀");
emotes.set(":D", "😀");
emotes.set(":p", "😛");
emotes.set(":P", "😛");
emotes.set(";(", "😭");
emotes.set(";)", "😉");
emotes.set(":o", "😮");
emotes.set(":O", "😮");

export const convertTextToEmotes = (text) => {
    emotes.forEach((value, key) => {
        text = text.replace(key, value);
    });

    return text;
};