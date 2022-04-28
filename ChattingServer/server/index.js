const ws = require('ws');

const wss = new ws.Server({port : 8080});

let id = 0;

wss.on("listening", () => {
    console.log("서버가 열렸습니다.");
});

wss.on("connection", socket => {
    id++;
    console.log("누군가가 서버에 접속했습니다.");
    socket.send(id + "번 접속 성공!");
    
    socket.on("message", messages => {
        socket.send(id + ": " + messages);
        console.log(id + ": " + messages);
    });
});