const ws = require('ws');
const fs = require('fs');

const wss = new ws.Server({port : 8080});

let scores = {};
const file = "./save.json";

if(fs.existsSync(file))
{
    scores = JSON.parse(fs.readFileSync(file));
}

function sendScores(socket)
{
    socket.send(Object.keys(scores).map((key) => {
        return key + "," + scores[key];
    }).join("|"));
}

wss.on('listening', () => {
    console.log ("서버가 시작되었습니다.");
});

wss.on('connection', socket => {
    console.log('유저가 접속!');
    socket.on('message', message => {
        const type = message.toString().split(':') [0];
        
        switch(type)
        {
            case "score":
                const data = message.toString().split(':')[1];
                const nickname = data.split(",")[0];
                const score = data.split(",")[1];

                scores[nickname] = score;
                wss.clients.forEach(client => {
                    sendScores(client);
                })
                fs.writeFileSync(file, JSON.stringify(scores));
                break;

            case "get":
                sendScores(socket);
                break;
        }    
    })
});