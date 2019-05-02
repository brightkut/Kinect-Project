console.log("server open");
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);

var room = []; // roomname 
var count_usr = [];
var vocab = ["Home"];
var vcheck ="";


http.listen(process.env.PORT || 80, function(){
  console.log('listening on this server heroku ');
});
//connect
io.on('connection', function(socket){
  


  //console.log('a user connected');
//server send to client
 	// io.emit('hi', "1");
//received and show data
//open server to status green
 	socket.on('open', function(msg){
    console.log("connected server");
    io.emit("alconnect","connected");
  });

 	socket.on('creRoom',function(roomname){
 		 var check =0;
 		for(i=0;i<room.length;i++){
 			if (room[i]==roomname) {
 				check+=1;
 			
 				break;
 			}
 		}
 		console.log("can't create room because this name is already exist");

 		if (check==0) {
 		room.push(roomname);
 		count_usr.push(0);
 		io.emit("notsame",roomname);
 	}
 	console.log(room);
 	console.log(count_usr);
 		
}
 	);
 	socket.on('join',function(roomname){
 		console.log("want to join this room name : "+ roomname);
 		var v =0;
 		var check = 0;
 		for(i=0;i<room.length;i++){
 			if (room[i]==roomname) {
 				if (count_usr[i]<2) {
 				v = count_usr[i];
 				count_usr[i]+=1;
 				
 				check+=1;
 				break;
 			}
 		}
 	}
 		if (check==1) {

 		socket.join(roomname);
 		io.in(roomname).emit("j",v);
 		console.log("alrady join room : " + roomname);
 		console.log(room);
 		console.log(count_usr);
 		console.log("connection person : " +v);
 	}else{
 		io.in(roomname).emit("e",null);
 	}


 	});
 	socket.on('c1',function(roomname,playerIndex,round){
 		console.log("playerIndex connect :"+ playerIndex);
 		

 	
 		if (playerIndex == 0) {
 			io.in(roomname).emit("first",null);

 		}else if (playerIndex==1) {
 			io.in(roomname).emit("second",null);
 			console.log("playerIndex 1 join room");
 		}
 	

 	});
 	socket.on('chat',function(vocab,roomname){
 		io.in(roomname).emit('chatb',vocab);
 		//console.log("send: " + vocab);
 	});
 	socket.on('s',function(msg,roomname){
 		if (msg==vcheck) {

 		}else
 		{
 		vcheck = msg;
 		io.in(roomname).emit('sb',msg);}

 	});
 /*
 	socket.on('v',function(round,playerIndex,word,roomname){
 		console.log("this is round : " + round);
 		console.log("this person is : " + playerIndex);
 		if (vocab[round]==word) {
 			io.sockets.to(roomname).emit('cor',"Wait");

 		}

 	});
 	*/



	socket.on('closed', function(roomname,name){
 		//users.splice(i,1);
 		if (room.length<=1) {
 			room.pop();
 			count_usr.pop();
 		}
 		else
 		{
 		for(i=0;i<room.length;i++){
 			if (room[i]==roomname) {
 				if (count_usr[i]==2) {
 					count_usr-=1;
 				}else{
 					roomname.splice(i,1);
 					count_usr.splice(i,1);
 				}
 			}

 		}
 	}
 		console.log(room);
 		console.log(count_usr);
 		io.emit("alclose",null);

  });



 	socket.on('display', function(body){
 		console.log(body);
 		//var obj = JSON.parse(body);
 		io.emit("show",body);
 		// body is string 
 		//console.log(typeof body);
 		
 		console.log("send joint already");

 	});

 
 	socket.on("start",function(name){
 		console.log(users);
 		for(i=0;i<users.length;i++){
 			if (name!=users[i]) {
 				io.emit("setOpp",users[i]);
 				break;
 			}
 		}
 		

 	});
 	
 		






});



