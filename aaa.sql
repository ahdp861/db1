-- Создание таблицы Students
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,             -- Уникальный идентификатор студента
    FirstName VARCHAR(50),                 -- Имя студента
    LastName VARCHAR(50),                  -- Фамилия студента
    Email VARCHAR(100),                    -- Электронная почта
    DateOfBirth DATE,                      -- Дата рождения
    EnrollmentDate DATE,                   -- Дата зачисления
    PhoneNumber CHAR(15),                  -- Номер телефона фиксированной длины
    IsActive BOOLEAN,                      -- Активный ли студент (true/false)
    GPA DECIMAL(3, 2),                     -- Средний балл (например, 3.75)
    GrantSum MONEY 							-- Размер стипендии
);

-- Создание таблицы Courses
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY,              -- Уникальный идентификатор курса
    CourseName VARCHAR(100),               -- Название курса
    Credits SMALLINT,                      -- Количество кредитов за курс
    StartDate DATE,                        -- Дата начала курса
    EndDate DATE,                          -- Дата окончания курса
    Description TEXT                       -- Описание курса (длинный текст)
);

-- Создание таблицы Enrollments (связь между Students и Courses)
CREATE TABLE Enrollments (
    EnrollmentID BIGINT PRIMARY KEY,       -- Уникальный идентификатор записи
    StudentID INT,                         -- Идентификатор студента (внешний ключ)
    CourseID INT,                          -- Идентификатор курса (внешний ключ)
    Grade FLOAT,                           -- Оценка студента по курсу (например, 85.5)
    EnrollmentDateTime TIMESTAMP,          -- Дата и время записи на курс
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO Students (StudentID, FirstName, LastName, Email, DateOfBirth, EnrollmentDate, PhoneNumber, IsActive, GPA, GrantSum)
VALUES 
(1, 'Иван', 'Иванов', 'ivan.ivanov@example.com', '2000-01-15', '2022-09-01', '1234567890', TRUE, 3.75, 5000.00),
(2, 'Мария', 'Петрова', 'maria.petrova@example.com', '1999-05-20', '2021-09-01', '0987654321', TRUE, 3.85, 6000.00),
(3, 'Алексей', 'Сидоров', 'alexey.sidorov@example.com', '2001-03-10', '2023-09-01', '1231231234', FALSE, 3.50, 0.00),
(4, 'Елена', 'Кузнецова', 'elena.kuznetsova@example.com', '2000-07-25', '2022-09-01', '3213214321', TRUE, 4.00, 5500.00),
(5, 'Дмитрий', 'Смирнов', 'dmitry.smirnov@example.com', '1998-11-30', '2020-09-01', '4564564567', TRUE, 3.90, 7000.00);

INSERT INTO Courses (CourseID, CourseName, Credits, StartDate, EndDate, Description)
VALUES 
(1, 'Введение в программирование', 5, '2025-02-01', '2025-06-30', 'Основы программирования на Python.'),
(2, 'Алгоритмы и структуры данных', 6, '2025-02-01', '2025-06-30', 'Изучение алгоритмов и структур данных.'),
(3, 'Базы данных', 4, '2025-02-01', '2025-06-30', 'Основы проектирования и работы с базами данных.'),
(4, 'Веб-разработка', 5, '2025-02-01', '2025-06-30', 'Создание веб-приложений с использованием HTML и CSS.'),
(5, 'Мобильная разработка', 6, '2025-02-01', '2025-06-30', 'Разработка мобильных приложений для Android и iOS.');

INSERT INTO Enrollments (EnrollmentID, StudentID, CourseID, Grade, EnrollmentDateTime)
VALUES 
(1, 1, 1, 85.0, CURRENT_TIMESTAMP),
(2, 1, 2, 90.0, CURRENT_TIMESTAMP),
(3, 2, 1, 88.0, CURRENT_TIMESTAMP),
(4, 2, 3, 92.0, CURRENT_TIMESTAMP),
(5, 3, 4, NULL, CURRENT_TIMESTAMP), -- Студент еще не получил оценку
(6, 4, 2, 95.0, CURRENT_TIMESTAMP),
(7, 4, 5, 89.0, CURRENT_TIMESTAMP),
(8, 5, 3, 80.0, CURRENT_TIMESTAMP),
(9, 5, 4, NULL , CURRENT_TIMESTAMP); -- Студент еще не получил оценку