export const emotes = new Map();

emotes.set(":)", "🙂");
emotes.set(":(", "😟");
emotes.set("<3", "❤");
emotes.set(":d", "😀");
emotes.set(":D", "😀");
emotes.set(":p", "😛");
emotes.set(":P", "😛");

export const convertTextToEmotes = (text) => {
    emotes.forEach((value, key) => {
        text = text.replace(key, value);
    });

    return text;
};