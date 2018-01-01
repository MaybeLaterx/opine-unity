var express = require('express');           // Used to easily direct requests
var mongodb = require('mongodb');
var mongojs = require('mongojs');           // Javascript interface to mongodb
var bodyParser = require('body-parser');    // Parses post request data for us
var config = require('./config');

var app = express();
var env = process.env.NODE_ENV || 'development';

// Tell the app to parse the body of incoming requests
app.use(bodyParser.urlencoded({extended:false}));
app.use(bodyParser.json());


// Create mongo connection
//app.db = mongojs('game_server', ['scores']); // localhost

var MongoClient = require('mongodb').MongoClient;
var db;
var uri = 'mongodb://jamiecarter7:D6li2mC81ZTGIi9i@dan-jamie-server-shard-00-00-il8yw.mongodb.net:27017,dan-jamie-server-shard-00-01-il8yw.mongodb.net:27017,dan-jamie-server-shard-00-02-il8yw.mongodb.net:27017/jjmadness?ssl=true&replicaSet=dan-jamie-server-shard-0&authSource=admin';  //###

// Reuse database object in request handlers
app.get("/test", function(req, res) {
  db.collection('users').find({}, function(err, data){
      console.log(data);
      res.end();
  });
});

	
	// SUBMIT VOTES
	app.post("/submitVotes", function(req, res) 
	{ 
		console.log(); 
		console.log('------------ Submit Votes ------------');
	
		// UUID - Tests
		if(!req.body.uuid)
		{
			res.send({error:"Submit Votes: Missing uuid"});
			console.log('Test UUID: Fail');
			return;
		}
		const uuid = req.body.uuid.toString();
		console.log('Test UUID: Pass');
	
		// Topics - Tests
		if(!req.body.topics)
		{
			res.send({error:"Submit Votes: Missing sent topics"});
			console.log('Test Topics: Fail');
			return;
		}
		const topicsReceived = req.body.topics;
		console.log('Test Topics: Pass (' + receivedTopics.length.toString() + ' topics received)');
		
		
		// call things you want 
		// call data from db on what topics user has already voted on 
		db.collection('users').find({uuid: uuid}, {topics: 1, _id:0}, function(err, result){

			if (err) { 
				res.send({error:"Submit Votes: Missing db topics"});
				console.log('Get existing user voted topics: Fail');
				return;
			}
			
			
			// if (result) { 
				//cycle through answers that the server received from player client
				for (i = 0; i < topicsReceived.length; i++) { 
					//compare user database to data received
					//check if entry already exists 
					const topicName = topicsReceived[i].id;
					const boolVote = topicsReceived[i].vote;
					let updateDatabase = true; 
					
					if (result.topics[topicName]) { // Entry already exists? 
						// if exists
						// update global topic ranking (+1 and -1)
						const existingBoolVote = result.topics.topicName;
						
						if (boolVote !== existingBoolVote) {
							
							if (boolVote) {
								db.collection('topics').update({topics: topicsReceived[i]}, {$inc : { likes : 1, dislikes : -1}});
								console.log(`Changed ${topicName} from dislike to like`);
							} else { 
								db.collection('topics').update({topics: topicsReceived[i]}, {$inc : { likes : -1, dislikes : 1}});
								console.log(`Changed ${topicName} from like to dislike`);
							}
						} else { 
							console.log(`Did not change vote on ${topicName}`);
							updateDatabase = false; 
						}

					} else { // Entry does not yet exist
						
						// update global topic value
						if (boolVote) {  // Liked! 
							db.collection('topics').update({topics: topicsReceived[i]}, {$inc : { likes : 1}});
							console.log(`Voted on ${topicName} for first time - liked!`);
						} else { // Disliked!
							db.collection('topics').update({topics: topicsReceived[i]}, {$inc : { dislikes : 1}});
							console.log(`Voted on ${topicName} for first time - disliked!`);
						}
						
					}
					
					// insert new value to topic
					if (updateDatabase) {
						db.collection('users').update({uuid: uuid}, {$set : {topics[topicName] : boolVote}}, {upsert: true},function(err){});
						console.log(`Updated ${uuid}'s vote on ${topicName}`);
					}
						
				}				
			// }
		
		});
		
		
		
		
		
		//############################DAY2##########################################################################################################################################
		
		// example: /getTopicsAndAnswers?batches=3&size=4	Returns 3 sets of 4 topics with answers (3 rounds of Pecking Order, for example)
		// example: /getTopicsAndAnswers?batches=10&size=1 	Returns 10 sets of 1 topic with an answer (10 rounds of Yay or Nay, for example)
		// only works for single player mode (?) 
		
		// GET TOPICS AND ANSWERS
		app.get("/getTopicsAndAnswers", function(req, res)
		{
			console.log();
			console.log('--------- Get Topics & Answers --------');

			// Get top 10 highscores
			// db.collection('highscores').find({}, {_id:0}).sort({score:-1, time: 1}).limit(10, function(err, result)
			
			if(!req.query.batches)
			{ 
				console.log("No batch value submitted");
				res.send({ success: false, data: {} })
				return;
			}
			
			if (req.query.batches < 0) { 
				console.log("Batch value less than 0");
				res.send({ success: false, data: {} });
				return;
			}
			
			if(!req.query.size)
			{ 
				console.log("No size value submitted");
				res.send({ success: false, data: {} })
				return;
			}
			
			if (req.query.size < 0) { 
				console.log("Size value less than 0");
				res.send({ success: false, data: {} });
				return;
			}
			
			let topicBatches[];
			let batchEntries[]; 
			 
			for (j = 0; j < req.query.batches; j++) { 
				batchEntries.empty(); 
				for (i = 0; i < req.query.size; i++) { 
					

					db.collection('topics').aggregate( { $sample : { size : 1 } }, function (err, result) { // Find one random entry to attempt to add
						batchEntries[i] = result; // Floor this? 
					} ); 
					
					for (k = 0; k < i; k++) { // Check it against all previous approval rates pulled so far (no close answers allowed) [[also catches topic duplicates]]
						thisRate = batchEntries[i].likes / (batchEntries[i].dislikes + batchEntries[i].likes); 
						thatRate = batchEntries[k].likes / (batchEntries[k].dislikes + batchEntries[k].likes); 
						diff = Math.abs(thisRate - thatRate);
						if (diff < 5) { // prevents answers being too close together
							i--; // force the iteration again, overwriting the answer
							break; // stop checking other entries for matches 
						}
					}
					
				}
				topicBatches[j] = batchEntries; 
			}
			
			res.send({success: true, data: { topics: topicBatches } });
			return;
		}
			
			
			
			
			
			
			
			db.collection('highscores').find({}, {_id:0}).sort({score:-1, time: 1}).limit(10).toArray(function(err, result)
			{
				if(err)
				{
					console.log("Failed to find scores: " + err);
					res.send({error:"Internal Server Error"});
					return;
				}
				const scores = result;
				console.log('Top 10 Scores Retrieved');
				//console.log(scores);

	// RANKING
	// No uuid submitted
	if(!req.query.uuid)
	{
	console.log("No user id submitted: Rank = -1")
	res.send({ success: true, data: { scores: scores, rank: -1 } })
	return;
	}
	// Find user info
	const uuid = req.query.uuid.toString()
	db.collection('highscores').findOne({ uuid: uuid }, { score: 1, name: 1, time: 1 }, function(err, result){
	if(err)
	{
	console.log("-- DB Error --");
						console.log(err);
	res.send({error: "Internal Server Error"});
	return;
	}

	// No highscore entry for user
	if (!result){
	console.log("Highscores: New user - no highscore")
	res.send({ success: true, data: { scores: scores, rank: -1 } })
	return
	}
					console.log(result);
	const currentHighscore = result.score
	const name = result.name
	const time = result.time

	// Find rank of user
	db.collection('highscores').count({
	$or: [
	{ score: { $gt: currentHighscore} }, // Greater than or equal to
	{ $and: [ { score: { $eq: currentHighscore}} , { time: { $lt: time} } ] } // Less than this time
	]},
	function(err, result){
	if(err)
	{
	console.log("Highscores: Failed to find rank - " + err);
	res.send({error:"Internal Server Error"});
	return;
	}
	var rank = result + 1;
	console.log(`Highscores: ${uuid} - ${name} is ranked ${rank}`);
	res.send({success: true, data: { scores: scores, rank: rank } });
	return;
	});
		});
			});
		});

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//############################ENDDAY2########################
		
		
		
		
		
		
		
		
		
		//###############################################################DAY3####################################################################
		
		
		
		 
		 
		 
		 // getVoteTopics?uuid=UNIQU3STR1NG&topics=10
		 app.post("/getVoteTopics", function(req, res)
		{
		 
			 // check uuid value given
			 if(!req.body.uuid)
			{ 
				console.log("No uuid value submitted");
				res.send({ success: false, data: {} })
				return;
			}
			
			 // check uuid exists 
			 // Find user info
			const uuid = req.query.uuid.toString()
			db.collection('users').findOne({ uuid: uuid }, /*{ score: 1, name: 1, time: 1 },*/ function(err, result){
				if(err)
				{
					console.log("UUID doesn't exist on database"); // this should probably input the new uuid instead of throwing an error 
					console.log(err);
					res.send({error: "Internal Server Error"});
					return;
				}
			}
		 
			if(!req.body.uuid)
			{ 
				console.log("No batch value submitted");
				res.send({ success: false, data: {} })
				return;
			}
		
			// check topics value given 
			if(!req.body.topics)
			{ 
				console.log("No topics value submitted");
				res.send({ success: false, data: {} })
				return;
			}
		 
			// check topics is more than 0	
			if (req.body.topics < 1) { 
				console.log("Topics value less than 1");
				res.send({ success: false, data: {} });
				return;
			} 
			
			 // Find all topics 
			 var allTopics = find all topics; 
			 
			 // Find all topics user has voted on
			 var usedVotedTopics = find all topics user has voted on; 
			 
			 // Find all topics user has not voted on yet
			 var newTopics = db.collection.aggregate( [{ $redact: { $cond: { if: {  $eq: [{	$size: {  $setDifference: ["$allTopics.topic", "$userVotedTopics.topic"]	}	  }, 0]	},	then: "$$KEEP", else: "$$PRUNE"  } } }, { $project: { _id: 0} }]	);
			
			 // Alternative method 
			  var newTopics = db.collection.aggregate( [{ $redact: { $cond: { if: {  $eq: [{	$size: {  $setIntersection: ["$allTopics.topic", "$userVotedTopics.topic"]	}	  }, 0]	},	then: "$$PRUNE", else: "$$KEEP"  } } }, { $project: { _id: 0} }]	);
			 
			 // redact = for EACH, perform prune/keep/descend	eq size = check that array returned is larger than 0 (i.e. successful)	setDifference = return values from array 1 that are not in array2	setIntersection = return values that are in both array 1 and 2		prune = remove from result		keep = keep in result
			 // shouldn't be .topic - aren't we accessing the name of the key?? Are both arrays structured identically? ###
			 
			 // pick up to 10 (or amount of topics requested)  from the newTopics list, unless this amount is lower than 10
			 var topicsToVoteOn = []; 
			 
			 // if length of array returned is smaller than total topics required, take sample of all topics to retrieve the number specified in the request (must be this amount at minimum)
			 if (result.length < req.body.topics) 
			 {
				// take all newTopics 
				topicsToVoteOn = newTopics; 
			 
				// get 10 (or however many specified) random entries
				db.collection('topics').aggregate( { $sample : { size : req.body.topics } }, function (err, result) { // ## there could be duplicates within here! 
					if (err) 
					{ 
						hgjfkdlsa;'#sfg;hlk
					}

					// Keep adding topics from this random set of topics until the requested number is reached
					for (i = 0; i < result.length; i++) 
					{ 
						// No duplicates from new topics already selected 
						boolean isDuplicate = false; 
						for (j = 0; j < newTopics.length; j++) 
						{ 
							if (newTopics[j] == result[i]) isDuplicate = true; 
						}
						
						if (!isDuplicate) 
						{ 
							newTopics.push(result[i]); 
							if (newTopics.length === req.body.topics) break; 
						}
					}
				} ); 
			 } else if (result.length === req.body.topics) { 
				// exact amount, take them all 
				topicsToVoteOn = newTopics;
			 } else { 
				// take 10 of the newTopics at random
				for (i = 0; i < req.body.topics; i++) { 
					// Randomly selected topic and add to final list
					var randomNumber = floor(Math.random() * req.body.topics); 
					topicsToVoteOn[i] = newTopics[randomNumber];
					
					// Remove selected topic from list of available topics 
					newTopics.splice(randomNumber, 1);
				}
			
			}
		 
		 var logText = "Sent ${req.body.topics} to vote on: "; 
		 
		 for (i = 0; i < topicsToVoteOn.length; i++) { 
			logText += ("${topicsToVoteOn[i]}, ")
		 }
		 
		 logText = logText.substring(0, logText.length-2); 
		 
		 console.log(logText);
		 res.send({success: true, data: {topics: topicsToVoteOn} });
          return;
		}
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 db.collection.aggregate(
		  [{
			$redact: {
			  $cond: {
				if: {
				  $eq: [{
					$size: {
					  $setIntersection: ["$FirstArray.Name", "$SecondArray.Name"]
					}
				  }, 0]
				},
				then: "$$KEEP",
				else: "$$PRUNE"
			  }
			}
		  }, {
			$project: {
			  _id: 1
			}
		  }]
		)
		
		db.experiments.aggregate(
   [
     { $project: { A: 1, B: 1, inBOnly: { $setDifference: [ "$B", "$A" ] }, _id: 0 } }  // sets variable inBOnly to the difference between collection B and A
   ]
)
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		db.users.insertMany(topics); // Update the user's preferences in the users collection - should change from ints to bools first ###
		
		// Update totalLikes or totalDislikes 
		
		// Has this user already voted on this topic? 
		bool alreadyVoted; 
		
		db.collection('users').findOne({ uuid: uuid}, function(err, result){

			if (err) console.log('USERERROR###');
			
			if (result) { 
				alreadyVoted = db.users.find( { uuid.topic : { $exists: true } } );
				alreadyVoted = true; 
				//result.topics[i]
			}
			// ### will alreadyVoted always be assigned one of these values? 
		}
		
		
		
		
		db.collection('users').update( {uuid: uuid}, {topics: topics}, { upsert: true }, function(err)
		
		
		
		
		let answers; 
		// Parse answers to int
		for (i = 0; i < tLength; i++) { 
			answers[i] = topics[i];
		}
		let score = parseInt(req.body.score);
		
		
		
		
		
		

// Is score a number
if(isNaN(req.body.score))
{
res.send({error:"Submit Score: Score value is not a number"});
            console.log('Test Score is a number: Fail');
return;
}
        console.log('Test Score is a Number: Pass');


// Does this user exist?
db.collection('highscores').findOne({ uuid: uuid }, function(err, result){
if(err)
            {
                console.log("Submit Score: Score error");
                res.send({error:"Internal Server Error"});
                return;
            }

            if (result){
                // If user exists
                console.log('Test User Exists: True');
                console.log(result);
	
	}
	
	
	
	
	
	
	
	
	
// SUBMIT SCORE (HIGHSCORE)
    app.post("/submitScore", function(req, res)
    {
        console.log();
        console.log('------------ Submit Score ------------');
// ----------------------
// Date
const time = Date.now()

// ----------------------
// UUID - Tests
if(!req.body.uuid)
{
res.send({error:"Submit Score: Missing uuid"});
            console.log('Test UUID: Fail');
return;
}
const uuid = req.body.uuid.toString();
        console.log('Test UUID: Pass');

// ----------------------
// Name - Tests
        if(!req.body.name)
        {
            res.send({error:"Submit Score: Missing name"});
            console.log('Test Name: Fail');
            return;
        }
const name = req.body.name.toString();
        console.log('Test Name: Pass');

// ---------------------
// Score - Tests
        if(!req.body.score)
        {
            res.send({error:"Submit Score: Missing score"});
            console.log('Test Score: Fail');

            return;
        }
        console.log('Test Score: Pass');

// Parse to int
let score = parseInt(req.body.score);

// Is score a number
if(isNaN(req.body.score))
{
res.send({error:"Submit Score: Score value is not a number"});
            console.log('Test Score is a number: Fail');
return;
}
        console.log('Test Score is a Number: Pass');


// Does this user exist?
db.collection('highscores').findOne({ uuid: uuid }, function(err, result){
if(err)
            {
                console.log("Submit Score: Score error");
                res.send({error:"Internal Server Error"});
                return;
            }

            if (result){
                // If user exists
                console.log('Test User Exists: True');
                console.log(result);

                // Get user current highscore
    const currentHighscore = result.score;
                console.log('Test Current Highscore: ' + currentHighscore);

                // Is this a highscore? NO!
                if (currentHighscore >= score){
                    console.log('Test Is This a highscore: Fail');

                    // Get time of highscore
                    const highscoreTime = result.time;

                    // Return User Rank
                    db.collection('highscores').count({
                                $or: [
                                    { score: { $gt: currentHighscore} }, // Score Greater Than
                                    { $and: [ { score: { $eq: currentHighscore}} , { time: { $lt: highscoreTime} } ] }, // Score eqaul to && Time less than this
                                ]},
                                 function(err, result){
                                    if(err)
                                    {
                                        console.log("Submit Score: Failed to find rank: " + err);
                                        res.send({error:"Internal Server Error"});
                                        return;
                                    }
                                    var rank = result + 1;
                                    res.send({success: true, data: {rank: rank} });
                                    console.log(`NO HIGHSCORE for ${name}, rank: ${rank}`);
                                    return;
                    });

                } else {
                    // Is this a highscore? YES!
                    // Update Highscore
                    console.log('Test Is This a highscore: True');

                    db.collection('highscores').update( {uuid: uuid}, {score: score, name: name, time: time, uuid: uuid}, { upsert: true }, function(err)
                    {
                        if(err)
                        {
                            console.log("Submit Score: Failed to insert score: " + err);
                            res.send({error:"Internal Server Error"});
                            return;
                        }

                        // Find rank of user
                        // Return User Rank
                        db.collection('highscores').count({
                                    $or: [
                                        { score: { $gt: score} }, // Score Greater Than
                                        { $and: [ { score: { $eq: score}} , { time: { $lt: time} } ] }, // Score eqaul to && Time less than this
                                    ]},
                                     function(err, result){
                                        if(err)
                                        {
                                            console.log("Submit Score: Failed to find rank: " + err);
                                            res.send({error:"Internal Server Error"});
                                            return;
                                        }
                                        var rank = result + 1;
                                        res.send({success: true, data: {rank: rank} });
                                        console.log(`NEW HIGHSCORE of ${score} for ${name}, rank: ${rank}`);
                                        return;
                        });
                    });
                }
            } else {
                // Update Highscore or where user is NOT found create new entry
                console.log('Test User Exists: False');

                db.collection('highscores').update( {uuid: uuid}, {score: score, name: name, time: time, uuid: uuid}, { upsert: true }, function(err)
                {
                    if(err)
                    {
                        console.log("Submit Score: Failed to insert score: " + err);
                        res.send({error:"Internal Server Error"});
                        return;
                    }
                    console.log(`New user ${name} created`);
                    // Find rank of user
                    db.collection('highscores').count({
                                $or: [
                                    { score: { $gt: score} }, // Greater than or equal to
                                    { $and: [ { score: { $eq: score}} , { time: { $lt: time} } ] } // Less than this time
                                ]},
                                 function(err, result){
                                    if(err)
                                    {
                                        console.log("Rank: Failed to find rank - " + err);
                                        res.send({error:"Internal Server Error"});
                                        return;
                                    }
                                    // var array = result.toArray();
                                    var rank = result + 1;
                                    console.log(`NEW USER ${name} scored a highscore ${score}, rank: ${rank}`);
                                    res.send({success: true, data: {rank: rank} });
                                    return;
                    });
                });
            }
});
    });

// GET RANK
    app.get("/rank", function(req, res)
    {
        console.log();
        console.log('-------------- Get Rank --------------');
if(!req.query.uuid)
{
console.log("Error: No unique id submitted");
res.send({error:"No unique id was submitted"});
return;
}
        console.log('Test UUID: Pass');
// Find user info
const uuid = req.query.uuid.toString()

db.collection('highscores').findOne({ uuid: uuid }, { score: 1, name: 1, time: 1 }, function(err, result){
if(err)
{
console.log("Error: Score error");
res.send({error:"Internal Server Error"});
return;
}
            //console.log(result);

// Yes -> Is this a highscore
if (!result){
console.log("No user: Rank -1")
res.send({ success: true, data: { rank:-1} })
return
}

const currentHighscore = result.score
const name = result.name
const time = result.time

db.collection('highscores').count({
$or: [
{ score: { $gt: currentHighscore} }, // Greater than or equal to
{ $and: [ { score: { $eq: currentHighscore}} , { time: { $lt: time} } ] } // Less than this time
]},
function(err, result){
if(err)
{
console.log("Rank: Failed to find rank - " + err);
res.send({error:"Internal Server Error"});
return;
}
                            // var array = result.toArray();
var rank = result + 1;
console.log(`Rank: ${name} is ranked ${rank}`);
res.send({success: true, data: {rank: rank} });
return;
});
    });
});

// GET HIGHSCORES and MY RANK
    app.get("/highScores", function(req, res)
    {
        console.log();
        console.log('--------- Get Highscore & Rank --------');

// Get top 10 highscores
        // db.collection('highscores').find({}, {_id:0}).sort({score:-1, time: 1}).limit(10, function(err, result)

        db.collection('highscores').find({}, {_id:0}).sort({score:-1, time: 1}).limit(10).toArray(function(err, result)
        {
            if(err)
            {
                console.log("Failed to find scores: " + err);
                res.send({error:"Internal Server Error"});
                return;
            }
            const scores = result;
            console.log('Top 10 Scores Retrieved');
            //console.log(scores);

// RANKING
// No uuid submitted
if(!req.query.uuid)
{
console.log("No user id submitted: Rank = -1")
res.send({ success: true, data: { scores: scores, rank: -1 } })
return;
}
// Find user info
const uuid = req.query.uuid.toString()
db.collection('highscores').findOne({ uuid: uuid }, { score: 1, name: 1, time: 1 }, function(err, result){
if(err)
{
console.log("-- DB Error --");
                    console.log(err);
res.send({error: "Internal Server Error"});
return;
}

// No highscore entry for user
if (!result){
console.log("Highscores: New user - no highscore")
res.send({ success: true, data: { scores: scores, rank: -1 } })
return
}
                console.log(result);
const currentHighscore = result.score
const name = result.name
const time = result.time

// Find rank of user
db.collection('highscores').count({
$or: [
{ score: { $gt: currentHighscore} }, // Greater than or equal to
{ $and: [ { score: { $eq: currentHighscore}} , { time: { $lt: time} } ] } // Less than this time
]},
function(err, result){
if(err)
{
console.log("Highscores: Failed to find rank - " + err);
res.send({error:"Internal Server Error"});
return;
}
var rank = result + 1;
console.log(`Highscores: ${uuid} - ${name} is ranked ${rank}`);
res.send({success: true, data: { scores: scores, rank: rank } });
return;
});
    });
        });
    });


    // SUBMIT NAME CHANGE
        app.post("/nameUpdate", function(req, res)
        {
            console.log();
            console.log('------------- Update Name -------------');

    // ----------------------
    // UUID - Tests
        // console.dir(req)
    if(!req.body.uuid)
    {
                console.log("Error: Missing uuid");
    res.send({error:"Error: Missing uuid"});
    return;
    }
            console.log('Test UUID: Pass');

    const uuid = req.body.uuid.toString();

    // ----------------------
    // Name - Tests
            if(!req.body.name)
            {
                console.log("Error: Missing name");
                res.send({error:"Error: Missing name"});
                return;
            }
            console.log('Test Name: Pass');
    const name = req.body.name.toString();


    // ---------------------
    // Does this user exist?
    db.collection('highscores').findOne({ uuid: uuid }, function(err, result){
    if(err)
                {
                    console.log("Name Update: Server Error");
                    res.send({error:"Internal Server Error"});
                    return;
                }
                // No account -> return success & do nothing
                console.log(result);
                if (!result){
                    console.log("Error: No account");
                    res.send({error: "error"}); //return error to disaude hackers
        return;
                }

                // Yes -> Change name
                // $set changes a key value otherwise all key values are overwritten with this one
            db.collection('highscores').update( {uuid: uuid}, {$set: {name: name}}, { upsert: false }, function(err)
            {
                if(err)
                {
                    console.log("Name Update: Error" + err);
                    res.send({error:"Internal Server Error"});
                    return;
                }
                    console.log(`Name Update: ${uuid} name changed to ${name}`);
                    res.send({error: "error"}); //return error to disaude hackers
                    return;
            });
    });



    //check if uuid exists
    // if yes, what is their score
    // is this a highscore?
    // update or insert highscore
    // return rank to to client


        });
//
// app.get("/rank", function(req, res)
//     {
// if(!req.query.uuid)
// {
// console.log("Rank: No unique id submitted");
// res.send({error:"No unique id was submitted"});
// return;
// }
// // Find user info
// const uuid = req.query.uuid.toString()
// db.collection('jjmadness').find({ uuid: uuid }, { score: 1, name: 1, time: 1 }, function(err, result){
// if(err)
// {
// console.log("Rank: Score error");
// res.send({error:"Internal Server Error"});
// return;
// }
//
// // Yes -> Is this a highscore
// if (result.length === 0){
// console.log("Rank: New user - no highscore")
// res.send({ success: true, data: { rank:-1} })
// return
// }
// const currentHighscore = result[0].score
// const name = result[0].name
// const time = result[0].time
//
// // Find rank of user
// db.collection('jjmadness').find({
// $or: [
// { score: { $gt: currentHighscore} }, // Greater than or equal to
// { $and: [ { score: { $eq: currentHighscore}} , { time: { $lt: time} } ] } // Less than this time
// ]},
// function(err, result){
// if(err)
// {
// console.log("Rank: Failed to find rank - " + err);
// res.send({error:"Internal Server Error"});
// return;
// }
// var rank = result.length + 1;
// console.log(`Rank: ${name} is ranked: ${rank}`);
// res.send({success: true, data: {rank: rank} });
// return;
// });
//      });
// });
//
// GET Stats
    app.get("/stats", function(req, res)
    {
const token = req.query.token
const accesstoken = 'nf8N*F$N$Ln0fn4w90fn0*niorw4nqr36829432whoopding'
if (token !== accesstoken){
console.log("Stats: Access Forbidden")
res.send({error: "Forbidden"});
return
}

// Number of highscores
var totalHighscores = 0;
db.collection('jjmadness').find({}, {_id:0}, function(err, result){
if(err)
{
console.log("Highscores: Failed to count highscores - " + err);
res.send({error:"Internal Server Error"});
return;
}
//console.log(`Total highscores: ${result.length}`);
totalHighscores = result.length;
})

// Recent Highscores 24hours?
const ago24hrs = Date.now() - 86400000;
var recentHighscores = 0;
db.collection('jjmadness').find({ time: { $gt: ago24hrs } }, {_id:0}, function(err, result){
if(err)
{
console.log("Recent: Failed to find recent highscores - " + err);
res.send({error:"Internal Server Error"});
return;
}
//console.log(`${result.length} new highscores in the last 24hours`);
var recentHighscores = result.length;
})
// Get top 10 highscores
console.log(recentHighscores);
res.send(
{success: true,
data: {
highscores: totalHighscores,
recent: recentHighscores,
}
}
);
        return;
    });

// ------
// Catch all other routes and return the index file
// MUST be last
app.get('*', (req, res) => {
res.send({error: "Forbidden"});
  return;
});

    app.post('*', (req, res) => {
        res.send({error: "Forbidden"});
        return;
    });

    // Initialize connection once
    MongoClient.connect(uri, function(err, database) {
      if(err) throw err;

      db = database;

      // Start the application after the database connection is ready
      // Start the server
       var server = app.listen(3000, '0.0.0.0', function()
        {
           console.log('Listening on port %d', server.address().port);
        });
    });
