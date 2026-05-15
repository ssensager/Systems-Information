CREATE TABLE users
(
    id SERIAL PRIMARY KEY,

    username VARCHAR(50) NOT NULL UNIQUE,

    password_hash TEXT NOT NULL,

    email VARCHAR(100) NOT NULL UNIQUE,

    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE books
(
    id SERIAL PRIMARY KEY,

    title VARCHAR(150) NOT NULL,

    author VARCHAR(100) NOT NULL,

    publication_year INT,

    owner_id INT NOT NULL,

    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_books_owner
        FOREIGN KEY(owner_id)
        REFERENCES users(id)
        ON DELETE CASCADE
);