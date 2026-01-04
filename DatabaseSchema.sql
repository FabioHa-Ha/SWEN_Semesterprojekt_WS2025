CREATE TABLE genres (
	genre_id	INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	name		VARCHAR(50) NOT NULL
);

CREATE TABLE users (
	user_id 		INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	username 		VARCHAR(50) NOT NULL UNIQUE,
	password 		VARCHAR(50) NOT NULL,
	salt_string 	VARCHAR(50) NOT NULL,
	email			VARCHAR(50),
	favorite_genre	INT REFERENCES genres(genre_id) ON DELETE CASCADE
);

CREATE TABLE media_types (
	media_type_name		VARCHAR(50) PRIMARY KEY
);

CREATE TABLE media_entries (
	media_entry_id 		INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	media_type			VARCHAR(50) NOT NULL REFERENCES media_types(media_type_name) ON DELETE CASCADE,
	title				VARCHAR(50),
	description			VARCHAR(500),
	release_year		INT,
	age_restriction		INT,
	creator				INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE favorite_media_entries (
	user_id 			INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
	media_entry_id 		INT NOT NULL REFERENCES media_entries(media_entry_id) ON DELETE CASCADE,
	PRIMARY KEY (user_id, media_entry_id)
);

CREATE TABLE media_entries_genres (
	media_entry_id		INT NOT NULL REFERENCES media_entries(media_entry_id) ON DELETE CASCADE,
	genre_id 			INT NOT NULL REFERENCES genres(genre_id) ON DELETE CASCADE,
	PRIMARY KEY (media_entry_id, genre_id)
);

CREATE TABLE ratings (
	rating_id				INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	creator					INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
	of_media_entry			INT NOT NULL REFERENCES media_entries(media_entry_id) ON DELETE CASCADE,
	star_rating				INT,
	rating_comment			VARCHAR(500),
	created_at 				TIMESTAMP DEFAULT current_timestamp,
	confirmed_by_author		BOOLEAN NOT NULL
);

CREATE TABLE rating_likes (
	rating_id		INT NOT NULL REFERENCES ratings(rating_id) ON DELETE CASCADE,
	user_id 		INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
	PRIMARY KEY (rating_id, user_id)
);

INSERT INTO media_types (media_type_name) VALUES ('Movie');
INSERT INTO media_types (media_type_name) VALUES ('Series');
INSERT INTO media_types (media_type_name) VALUES ('Game');

INSERT INTO users (username, password, salt_string) VALUES ('user1', '9EwhQcJ3iMB50R0e3lrr8vx5RLZ0tQT0lIEFcYIZna0=', '03ZSRGFD4HKAJ97J5TC0'); -- 1234
INSERT INTO users (username, password, salt_string) VALUES ('user2', 'v+5FHWd2CwZm9EKwJwk0PhD9/ccP7zqTrIzMwF7O+m4=', '2702MVI4NNRKKL2OR72Q'); -- abcd
INSERT INTO users (username, password, salt_string) VALUES ('user3', '8xH8J8aHOmMWWyAup80tEkEop2sAFPxSczATeJbUHZ8=', 'IXYLQHRSOL5C0O7SJXR2'); -- password

INSERT INTO genres (name) VALUES ('Sci Fi');
INSERT INTO genres (name) VALUES ('Fantasy');
INSERT INTO genres (name) VALUES ('Action');
INSERT INTO genres (name) VALUES ('Cartoon');
INSERT INTO genres (name) VALUES ('FPS');
INSERT INTO genres (name) VALUES ('Open World');

INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Movie', 'Dune', 'A visually ambitious, slow‐burn adaptation that follows Paul Atreidess fall from aristocratic heir to desert exile and messianic figure, set against a backdrop of interstellar politics, resource warfare and ecological struggle on the spice‐rich planet Arrakis.',
		2021, 13, 1);
INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Movie', 'Iron Man', 'Iron Man is one of many Marvel heroes with a genius-level intellect, but his focus on societal application alongside hard science distinguishes him from other heroes. The character is a futurist, and he works to identify solutions for problems that have yet to emerge.', 
		2008, 13, 3);
INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Series', 'Gravity Falls', 'The series follows the adventures of Dipper Pines and his twin sister Mabel, who are sent to spend the summer with their great-uncle (or "Grunkle") Stan in Gravity Falls, Oregon, a mysterious town rife with paranormal incidents and supernatural creatures.',
		2012, 8, 2);
INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Series', 'Percy Jackson and the Olympians', 'Percy Jackson and the Olympians follows Percy, a troubled tween with unexplainable powers. In each installment, Percy finds himself face-to-face with formidable new foes as he comes into his own, amassing loyal allies and burgeoning new abilities throughout his journey.', 
		2023, 9, 2);
INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Game', 'Overwatch', 'Described as a "hero shooter", Overwatch assigned players into two teams of six, with each player selecting from a large roster of characters, known as "heroes", with unique abilities. Teams worked to complete map-specific objectives within a limited period of time.', 
		2016, 13, 1);
INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) 
	VALUES ('Game', 'Minecraft', 'Minecraft is a 3D sandbox video game that has no required goals to accomplish, allowing players a large amount of freedom in choosing how to play the game.', 
		2011, 10, 2);

INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (1, 1);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (2, 3);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (3, 2);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (3, 4);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (4, 2);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (4, 3);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (5, 3);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (5, 5);
INSERT INTO media_entries_genres (media_entry_id, genre_id) VALUES (6, 6);

INSERT INTO ratings (creator, of_media_entry, star_rating, confirmed_by_author)
	VALUES (2, 1, 4, true);
INSERT INTO ratings (creator, of_media_entry, star_rating, rating_comment, confirmed_by_author)
	VALUES (2, 2, 3, 'Overrated', false);
INSERT INTO ratings (creator, of_media_entry, star_rating, rating_comment, confirmed_by_author)
	VALUES (1, 6, 5, 'Best game ever', true);
INSERT INTO ratings (creator, of_media_entry, star_rating, rating_comment, confirmed_by_author)
	VALUES (2, 6, 5, 'Great memories!', true);

INSERT INTO rating_likes (rating_id, user_id) VALUES (1, 1);
INSERT INTO rating_likes (rating_id, user_id) VALUES (3, 2);
INSERT INTO rating_likes (rating_id, user_id) VALUES (3, 3);

INSERT INTO favorite_media_entries (media_entry_id, user_id) VALUES (1, 2);
INSERT INTO favorite_media_entries (media_entry_id, user_id) VALUES (5, 2);
INSERT INTO favorite_media_entries (media_entry_id, user_id) VALUES (6, 2);
INSERT INTO favorite_media_entries (media_entry_id, user_id) VALUES (6, 1);