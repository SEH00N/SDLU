//- 서버
const ws = require('ws');
const fs = require('fs');

const wss = new ws.Server({port : 8080}); //8080포트로 웹소켓서버 열기

let scores = {}; //스코어 배열
const file = "./save.json"; //스코어 정보를 저장할 파일 경로

if(fs.existsSync(file)) // "file" 폴더가 있을 떄
{
    scores = JSON.parse(fs.readFileSync(file)); //scores 배열의 값에 "file"폴더의 값을 저장
}

function sendScores(socket) //Scores 의 값을 소켓에 보내주는 함수
{
    socket.send(Object.keys(scores).map((key) => {
        return key + "," + scores[key];
    }).join("|"));
}

//웹소켓서버가 시작되었을 때
wss.on('listening', () => {
    console.log ("서버가 시작되었습니다.");
});

//웹소켓서버에 클라이언트가 접속했을 때
wss.on('connection', socket => {
    console.log('유저가 접속!');
    sendScores(socket);
    socket.on('message', message => {
        const type = message.toString().split(':') [0]; //message변수에서 ":"를 중심으로 잘라 [0] 자리의 값을 type변수에 저장
        
        switch(type)
        {
            case "score":
                const data = message.toString().split(':')[1]; //message변수에서 ":"를 중심으로 잘라 [1] 자리의 값을 data변수에 저장
                const nickname = data.split(",")[0]; //data변수에서 ","를 중심으로 잘라 [0] 자리의 값을 nickname변수에 저장
                const score = data.split(",")[1]; //data변수에서 ","를 중심으로 잘라 [1] 자리의 값을 score변수에 저장

                scores[nickname] = score; //scores 딕셔너리에 nickname = score 저장

                //배열을 처음부터 끝까지 돌면서 client에 저장하고 client에 scores 값 보내기
                wss.clients.forEach(client => {
                    sendScores(client);
                })
                //"file"위치의 파일에 scores 값 저장
                fs.writeFileSync(file, JSON.stringify(scores));
                break;

            case "get":
                sendScores(socket); //소켓에 scores 값 보내기
                break;
        }    
    })
});