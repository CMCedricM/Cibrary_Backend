CREATE TABLE Users (
  id serial Primary key,
  UserName text,
  Email text,
  FirstName text,
  LastName text,
  LastLogin timestamp
);

CREATE TABLE Books(
  id serial Primary key,
  ISBN text,
  Title text,
  ReleaseDate timestamp null,
  AvailabilityCnt integer,
  TotalCnt integer
);

CREATE TYPE book_status AS ENUM ('returned', 'checked-out', 'overdue');

CREATE TABLE Circulation(
  id serial Primary key, 
  User_Id integer, 
  Book_Id integer, 
  Checkout_Date timestamp,
  Due_Date timestamp,
  Return_Date timestamp null,
  Status book_status default 'checked-out',
  CONSTRAINT fk_User FOREIGN KEY (User_Id)
  REFERENCES Users(id),
  CONSTRAINT fk_Books FOREIGN KEY (Book_Id) 
  REFERENCES Books(id) 
);

CREATE INDEX idx_user_emails ON Users(Email);
CREATE INDEX idx_isbn_number ON Books(ISBN);