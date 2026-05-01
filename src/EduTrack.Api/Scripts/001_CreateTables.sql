CREATE TABLE Student (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(200) NOT NULL,
	Email VARCHAR(200) NOT NULL UNIQUE
);

CREATE TABLE Course (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(200) NOT NULL
);

CREATE TABLE Enrollment (
	StudentId INT NOT NULL,
	CourseId INT NOT NULL,
	EnrollmentDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT FK_Enrollment_Student FOREIGN KEY (StudentId) REFERENCES Student(Id),
	CONSTRAINT FK_Enrollment_Course FOREIGN KEY (CourseId) REFERENCES Course(Id),

	-- Constraint to prevent a student from enrolling in the same course twice 
	CONSTRAINT PK_Enrollment PRIMARY KEY (StudentId, CourseId)
);
	