CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TYPE user_status AS ENUM ('basic', 'admin', 'founder');

CREATE TABLE Users (
  id serial Primary key,
  auth0id text,
  role user_status default 'basic',
  name text,
  email text,
  username text,
  phone_number text,
  FirstName text, 
  LastName text,
  LastLogin timestamp
);

CREATE TABLE Books(
  id text Primary key,
  uuid UUID default gen_random_uuid(),
  isbn text NOT NULL,
  Title text,
  ReleaseDate timestamp null,
  created_at timestamp default CURRENT_TIMESTAMP,
  AvailabilityCnt integer,
  TotalCnt integer,
  description text
);

CREATE TYPE book_status AS ENUM ('returned', 'checked_out', 'overdue', 'pending');

CREATE TABLE Circulation(
  id serial Primary key, 
  User_Id integer, 
  Book_Id text, 
  Checkout_Date timestamp,
  Due_Date timestamp,
  Return_Date timestamp null,
  Status book_status default 'pending',
  CONSTRAINT fk_users FOREIGN KEY (User_Id)
  REFERENCES Users(id),
  CONSTRAINT fk_book FOREIGN KEY (Book_Id) 
  REFERENCES Books(id) 
);


CREATE or REPLACE FUNCTION gen_book_id()
RETURNS TRIGGER AS $$
BEGIN 
  if NEW.uuid IS NULL THEN 
    NEW.uuid := gen_random_uuid();
  END IF;

  NEW.id := encode(digest(COALESCE(NEW.isbn, '') || COALESCE(NEW.created_at::text, '') || COALESCE(NEW.uuid::text, ''), 'sha256'), 'base64');
  RETURN NEW; 
END; 
$$ LANGUAGE plpgsql;

CREATE TRIGGER books_id_hash
BEFORE INSERT ON Books
FOR EACH ROW 
EXECUTE FUNCTION gen_book_id(); 

CREATE INDEX idx_user_emails ON Users(Email);
CREATE INDEX idx_isbn_number ON Books(isbn);
CREATE INDEX idx_user_role ON Users(role);