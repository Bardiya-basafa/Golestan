// Sample data
let courses = [
    {id: 1, code: 'CS101', name: 'Introduction to Programming', credits: 3, prerequisite: null},
    {id: 2, code: 'CS201', name: 'Data Structures', credits: 3, prerequisite: 1},
    {id: 3, code: 'MATH101', name: 'Calculus I', credits: 4, prerequisite: null},
    {id: 4, code: 'CS301', name: 'Algorithms', credits: 3, prerequisite: 2}
];

let classes = [
    {id: 1, courseId: 1, professorId: 1, day: 'Monday', startTime: '08:00', endTime: '10:00', location: 'Room 101'},
    {id: 2, courseId: 2, professorId: 1, day: 'Wednesday', startTime: '10:00', endTime: '12:00', location: 'Lab 201'},
    {id: 3, courseId: 3, professorId: 2, day: 'Tuesday', startTime: '14:00', endTime: '16:00', location: 'Room 301'}
];

let professors = [
    {id: 1, name: 'Dr. Smith', username: 'prof.smith', faculty: 'Computer Science', password: 'password123'},
    {id: 2, name: 'Dr. Johnson', username: 'prof.johnson', faculty: 'Mathematics', password: 'password123'}
];

let students = [
    {
        id: 1,
        name: 'John Doe',
        studentId: '2024001',
        username: 'john.doe',
        faculty: 'Computer Science',
        password: 'password123'
    },
    {
        id: 2,
        name: 'Jane Smith',
        studentId: '2024002',
        username: 'jane.smith',
        faculty: 'Mathematics',
        password: 'password123'
    }
];

// Initialize
document.addEventListener('DOMContentLoaded', function () {
    updateStats();
    updateCourseOptions();
    updatePrerequisiteOptions();
});

function updateStats() {
    document.getElementById('totalCourses').textContent = courses.length;
    document.getElementById('totalClasses').textContent = classes.length;
    document.getElementById('totalProfessors').textContent = professors.length;
    document.getElementById('totalStudents').textContent = students.length;
}

// Navigation functions
function showSection(sectionId) {
    document.getElementById('dashboardView').classList.add('hidden');
    document.getElementById('contentArea').classList.remove('hidden');

    // Hide all sections
    const sections = document.querySelectorAll('#contentArea > div:not(:first-child)');
    sections.forEach(section => section.classList.add('hidden'));

    // Show selected section
    document.getElementById(sectionId).classList.remove('hidden');

    // Load section data
    loadSectionData(sectionId);
}

function goBack() {
    document.getElementById('dashboardView').classList.remove('hidden');
    document.getElementById('contentArea').classList.add('hidden');
}

function loadSectionData(sectionId) {
    switch (sectionId) {
        case 'courseManagement':
            loadCoursesTable();
            break;
        case 'classManagement':
            loadClassesTable();
            break;
        case 'professorManagement':
            loadProfessorsTable();
            break;
        case 'studentManagement':
            loadStudentsTable();
            break;
    }
}

function loadCoursesTable() {
    const tbody = document.getElementById('coursesTable');
    tbody.innerHTML = '';

    courses.forEach(course => {
        const prerequisiteName = course.prerequisite ?
            courses.find(c => c.id === course.prerequisite)?.name || 'Unknown' :
            'None';

        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">${course.code}</td>
                        <td class="py-4 px-4 text-gray-700">${course.name}</td>
                        <td class="py-4 px-4 text-gray-700">${course.credits}</td>
                        <td class="py-4 px-4 text-gray-700">${prerequisiteName}</td>
                        <td class="py-4 px-4">
                            <div class="flex space-x-2">
                                <button onclick="editCourse(${course.id})" class="text-blue-600 hover:text-blue-800 font-medium">
                                    <i class="fas fa-edit mr-1"></i>Edit
                                </button>
                                <button onclick="deleteCourse(${course.id})" class="text-red-600 hover:text-red-800 font-medium">
                                    <i class="fas fa-trash mr-1"></i>Delete
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });
}

function loadClassesTable() {
    const tbody = document.getElementById('classesTable');
    tbody.innerHTML = '';

    classes.forEach(cls => {
        const course = courses.find(c => c.id === cls.courseId);
        const professor = professors.find(p => p.id === cls.professorId);

        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">CLS${cls.id.toString().padStart(3, '0')}</td>
                        <td class="py-4 px-4 text-gray-700">${course?.name || 'Unknown'}</td>
                        <td class="py-4 px-4 text-gray-700">${professor?.name || 'Not Assigned'}</td>
                        <td class="py-4 px-4 text-gray-700">${cls.day} ${cls.startTime}-${cls.endTime}</td>
                        <td class="py-4 px-4 text-gray-700">${cls.location}</td>
                        <td class="py-4 px-4">
                            <div class="flex space-x-2">
                                <button onclick="editClass(${cls.id})" class="text-blue-600 hover:text-blue-800 font-medium">
                                    <i class="fas fa-edit mr-1"></i>Edit
                                </button>
                                <button onclick="deleteClass(${cls.id})" class="text-red-600 hover:text-red-800 font-medium">
                                    <i class="fas fa-trash mr-1"></i>Delete
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });
}

function loadProfessorsTable() {
    const tbody = document.getElementById('professorsTable');
    tbody.innerHTML = '';

    professors.forEach(professor => {
        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">PROF${professor.id.toString().padStart(3, '0')}</td>
                        <td class="py-4 px-4 text-gray-700">${professor.name}</td>
                        <td class="py-4 px-4 text-gray-700">${professor.faculty}</td>
                        <td class="py-4 px-4 text-gray-700">${professor.username}</td>
                        <td class="py-4 px-4">
                            <div class="flex space-x-2">
                                <button onclick="editProfessor(${professor.id})" class="text-blue-600 hover:text-blue-800 font-medium">
                                    <i class="fas fa-edit mr-1"></i>Edit
                                </button>
                                <button onclick="deleteProfessor(${professor.id})" class="text-red-600 hover:text-red-800 font-medium">
                                    <i class="fas fa-trash mr-1"></i>Delete
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });
}

function loadStudentsTable() {
    const tbody = document.getElementById('studentsTable');
    tbody.innerHTML = '';

    students.forEach(student => {
        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">${student.studentId}</td>
                        <td class="py-4 px-4 text-gray-700">${student.name}</td>
                        <td class="py-4 px-4 text-gray-700">${student.faculty}</td>
                        <td class="py-4 px-4 text-gray-700">${student.username}</td>
                        <td class="py-4 px-4">
                            <div class="flex space-x-2">
                                <button onclick="editStudent(${student.id})" class="text-blue-600 hover:text-blue-800 font-medium">
                                    <i class="fas fa-edit mr-1"></i>Edit
                                </button>
                                <button onclick="deleteStudent(${student.id})" class="text-red-600 hover:text-red-800 font-medium">
                                    <i class="fas fa-trash mr-1"></i>Delete
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });
}

// Modal functions
function showModal(modalId) {
    document.getElementById(modalId).classList.remove('hidden');
}

function hideModal(modalId) {
    document.getElementById(modalId).classList.add('hidden');
}

// Form handlers
document.getElementById('addCourseForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const courseData = {
        id: courses.length + 1,
        code: document.getElementById('courseCode').value,
        name: document.getElementById('courseName').value,
        credits: parseInt(document.getElementById('courseCredits').value),
        prerequisite: document.getElementById('coursePrerequisite').value || null
    };

    courses.push(courseData);
    loadCoursesTable();
    updateStats();
    updateCourseOptions();
    updatePrerequisiteOptions();
    hideModal('addCourseModal');
    this.reset();
    showNotification('Course added successfully!', 'success');
});

document.getElementById('addClassForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const classData = {
        id: classes.length + 1,
        courseId: parseInt(document.getElementById('classCourse').value),
        professorId: null,
        day: document.getElementById('classDay').value,
        startTime: document.getElementById('classStartTime').value,
        endTime: document.getElementById('classEndTime').value,
        location: document.getElementById('classLocation').value
    };

    // Check for conflicts
    const hasConflict = classes.some(cls =>
        cls.day === classData.day &&
        cls.location === classData.location &&
        ((classData.startTime >= cls.startTime && classData.startTime < cls.endTime) ||
            (classData.endTime > cls.startTime && classData.endTime <= cls.endTime))
    );

    if (hasConflict) {
        showNotification('Time or location conflict detected!', 'error');
        return;
    }

    classes.push(classData);
    loadClassesTable();
    updateStats();
    hideModal('addClassModal');
    this.reset();
    showNotification('Class added successfully!', 'success');
});

document.getElementById('addProfessorForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const professorData = {
        id: professors.length + 1,
        name: document.getElementById('professorName').value,
        faculty: document.getElementById('professorFaculty').value,
        username: document.getElementById('professorUsername').value,
        password: document.getElementById('professorPassword').value
    };

    professors.push(professorData);
    loadProfessorsTable();
    updateStats();
    hideModal('addProfessorModal');
    this.reset();
    showNotification('Professor added successfully!', 'success');
});

document.getElementById('addStudentForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const studentData = {
        id: students.length + 1,
        name: document.getElementById('studentName').value,
        studentId: document.getElementById('studentId').value,
        faculty: document.getElementById('studentFaculty').value,
        username: document.getElementById('studentUsername').value,
        password: document.getElementById('studentPassword').value
    };

    students.push(studentData);
    loadStudentsTable();
    updateStats();
    hideModal('addStudentModal');
    this.reset();
    showNotification('Student added successfully!', 'success');
});

// Update options
function updateCourseOptions() {
    const select = document.getElementById('classCourse');
    select.innerHTML = '<option value="">Select Course</option>';

    courses.forEach(course => {
        const option = document.createElement('option');
        option.value = course.id;
        option.textContent = `${course.code} - ${course.name}`;
        select.appendChild(option);
    });
}

function updatePrerequisiteOptions() {
    const select = document.getElementById('coursePrerequisite');
    select.innerHTML = '<option value="">No Prerequisites</option>';

    courses.forEach(course => {
        const option = document.createElement('option');
        option.value = course.id;
        option.textContent = `${course.code} - ${course.name}`;
        select.appendChild(option);
    });
}

// CRUD operations
function deleteCourse(courseId) {
    if (confirm('Are you sure you want to delete this course?')) {
        courses = courses.filter(c => c.id !== courseId);
        loadCoursesTable();
        updateStats();
        updateCourseOptions();
        updatePrerequisiteOptions();
        showNotification('Course deleted successfully!', 'success');
    }
}

function deleteClass(classId) {
    if (confirm('Are you sure you want to delete this class?')) {
        classes = classes.filter(c => c.id !== classId);
        loadClassesTable();
        updateStats();
        showNotification('Class deleted successfully!', 'success');
    }
}

function deleteProfessor(professorId) {
    if (confirm('Are you sure you want to delete this professor?')) {
        professors = professors.filter(p => p.id !== professorId);
        loadProfessorsTable();
        updateStats();
        showNotification('Professor deleted successfully!', 'success');
    }
}

function deleteStudent(studentId) {
    if (confirm('Are you sure you want to delete this student?')) {
        students = students.filter(s => s.id !== studentId);
        loadStudentsTable();
        updateStats();
        showNotification('Student deleted successfully!', 'success');
    }
}

// Notification system
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 z-50 p-4 rounded-xl shadow-lg animate-bounce-in ${
        type === 'success' ? 'bg-green-500' :
            type === 'error' ? 'bg-red-500' : 'bg-blue-500'
    } text-white`;
    notification.innerHTML = `
                <div class="flex items-center">
                    <i class="fas fa-${type === 'success' ? 'check' : type === 'error' ? 'times' : 'info'} mr-2"></i>
                    ${message}
                </div>
            `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

tailwind.config = {
    theme: {
        extend: {
            colors: {
                primary: {
                    50: '#eff6ff',
                    500: '#3b82f6',
                    600: '#2563eb',
                    700: '#1d4ed8',
                    900: '#1e3a8a'
                }
            },
            animation: {
                'fade-in': 'fadeIn 0.5s ease-out',
                'slide-up': 'slideUp 0.3s ease-out',
                'bounce-in': 'bounceIn 0.6s ease-out'
            },
            keyframes: {
                fadeIn: {
                    '0%': {opacity: '0', transform: 'translateY(10px)'},
                    '100%': {opacity: '1', transform: 'translateY(0)'}
                },
                slideUp: {
                    '0%': {transform: 'translateY(100%)'},
                    '100%': {transform: 'translateY(0)'}
                },
                bounceIn: {
                    '0%': {transform: 'scale(0.3)', opacity: '0'},
                    '50%': {transform: 'scale(1.05)'},
                    '70%': {transform: 'scale(0.9)'},
                    '100%': {transform: 'scale(1)', opacity: '1'}
                }
            }
        }
    }
}


// Sample data
const currentProfessorId = 1; // Dr. Smith

let courses = [
    {id: 1, code: 'CS101', name: 'Introduction to Programming', credits: 3},
    {id: 2, code: 'CS201', name: 'Data Structures', credits: 3},
    {id: 3, code: 'MATH101', name: 'Calculus I', credits: 4}
];

let classes = [
    {id: 1, courseId: 1, professorId: 1, day: 'Monday', startTime: '08:00', endTime: '10:00', location: 'Room 101'},
    {id: 2, courseId: 2, professorId: 1, day: 'Wednesday', startTime: '10:00', endTime: '12:00', location: 'Lab 201'}
];

let students = [
    {id: 1, name: 'John Doe', studentId: '2024001', faculty: 'Computer Science'},
    {id: 2, name: 'Jane Smith', studentId: '2024002', faculty: 'Computer Science'},
    {id: 3, name: 'Bob Johnson', studentId: '2024003', faculty: 'Computer Science'}
];

let enrollments = [
    {studentId: 1, classId: 1},
    {studentId: 2, classId: 1},
    {studentId: 3, classId: 1},
    {studentId: 1, classId: 2},
    {studentId: 2, classId: 2}
];

let grades = [
    {studentId: 1, classId: 1, midterm: 18.5, final: 19.0},
    {studentId: 2, classId: 1, midterm: 16.0, final: 17.5},
    {studentId: 3, classId: 1, midterm: null, final: null},
    {studentId: 1, classId: 2, midterm: 17.0, final: null},
    {studentId: 2, classId: 2, midterm: null, final: null}
];

let currentEditingGrade = null;

// Initialize
document.addEventListener('DOMContentLoaded', function () {
    updateStats();
    loadClassSelects();
});

function updateStats() {
    const myClasses = classes.filter(c => c.professorId === currentProfessorId);
    const totalStudents = enrollments.filter(e =>
        myClasses.some(c => c.id === e.classId)
    ).length;
    const pendingGrades = grades.filter(g =>
        myClasses.some(c => c.id === g.classId) && (g.midterm === null || g.final === null)
    ).length;

    document.getElementById('myClasses').textContent = myClasses.length;
    document.getElementById('totalStudents').textContent = totalStudents;
    document.getElementById('pendingGrades').textContent = pendingGrades;
}

function loadClassSelects() {
    const myClasses = classes.filter(c => c.professorId === currentProfessorId);

    const gradeSelect = document.getElementById('gradeClassSelect');
    const studentSelect = document.getElementById('studentClassSelect');

    [gradeSelect, studentSelect].forEach(select => {
        select.innerHTML = '<option value="">Select a class</option>';
        myClasses.forEach(cls => {
            const course = courses.find(c => c.id === cls.courseId);
            const option = document.createElement('option');
            option.value = cls.id;
            option.textContent = `${course.code} - ${course.name} (${cls.day} ${cls.startTime}-${cls.endTime})`;
            select.appendChild(option);
        });
    });
}

// Navigation functions
function showSection(sectionId) {
    document.getElementById('dashboardView').classList.add('hidden');
    document.getElementById('contentArea').classList.remove('hidden');

    // Hide all sections
    const sections = document.querySelectorAll('#contentArea > div:not(:first-child)');
    sections.forEach(section => section.classList.add('hidden'));

    // Show selected section
    document.getElementById(sectionId).classList.remove('hidden');

    // Load section data
    loadSectionData(sectionId);
}

function goBack() {
    document.getElementById('dashboardView').classList.remove('hidden');
    document.getElementById('contentArea').classList.add('hidden');
}

function loadSectionData(sectionId) {
    switch (sectionId) {
        case 'professorClasses':
            loadProfessorClasses();
            break;
    }
}

function loadProfessorClasses() {
    const myClasses = classes.filter(c => c.professorId === currentProfessorId);
    const container = document.getElementById('classCards');
    container.innerHTML = '';

    myClasses.forEach(cls => {
        const course = courses.find(c => c.id === cls.courseId);
        const enrolledStudents = enrollments.filter(e => e.classId === cls.id).length;

        const card = `
                    <div class="bg-white rounded-xl p-6 shadow-md border border-gray-100 card-hover">
                        <div class="flex items-center justify-between mb-4">
                            <div class="w-12 h-12 bg-gradient-to-r from-green-500 to-teal-500 rounded-lg flex items-center justify-center">
                                <i class="fas fa-book text-white"></i>
                            </div>
                            <span class="text-sm font-medium text-gray-500">${enrolledStudents} students</span>
                        </div>
                        <h4 class="text-lg font-bold text-gray-800 mb-2">${course.code}</h4>
                        <p class="text-gray-600 mb-3">${course.name}</p>
                        <div class="space-y-2 text-sm text-gray-500">
                            <div class="flex items-center">
                                <i class="fas fa-calendar mr-2"></i>
                                ${cls.day}
                            </div>
                            <div class="flex items-center">
                                <i class="fas fa-clock mr-2"></i>
                                ${cls.startTime} - ${cls.endTime}
                            </div>
                            <div class="flex items-center">
                                <i class="fas fa-map-marker-alt mr-2"></i>
                                ${cls.location}
                            </div>
                        </div>
                    </div>
                `;
        container.innerHTML += card;
    });
}

function loadStudentsForGrading() {
    const classId = parseInt(document.getElementById('gradeClassSelect').value);
    if (!classId) {
        document.getElementById('gradeTable').classList.add('hidden');
        return;
    }

    const classStudents = enrollments.filter(e => e.classId === classId);
    const tbody = document.getElementById('gradeTableBody');
    tbody.innerHTML = '';

    classStudents.forEach(enrollment => {
        const student = students.find(s => s.id === enrollment.studentId);
        const grade = grades.find(g => g.studentId === student.id && g.classId === classId) ||
            {studentId: student.id, classId: classId, midterm: null, final: null};

        const totalGrade = (grade.midterm !== null && grade.final !== null) ?
            ((grade.midterm * 0.4) + (grade.final * 0.6)).toFixed(1) : '-';

        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">${student.studentId}</td>
                        <td class="py-4 px-4 text-gray-700">${student.name}</td>
                        <td class="py-4 px-4 text-gray-700">${grade.midterm || '-'}</td>
                        <td class="py-4 px-4 text-gray-700">${grade.final || '-'}</td>
                        <td class="py-4 px-4 font-medium text-gray-900">${totalGrade}</td>
                        <td class="py-4 px-4">
                            <button onclick="editGrade(${student.id}, ${classId})" class="text-blue-600 hover:text-blue-800 font-medium">
                                <i class="fas fa-edit mr-1"></i>Edit Grade
                            </button>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });

    document.getElementById('gradeTable').classList.remove('hidden');
}

function loadStudentsList() {
    const classId = parseInt(document.getElementById('studentClassSelect').value);
    if (!classId) {
        document.getElementById('studentListTable').classList.add('hidden');
        return;
    }

    const classStudents = enrollments.filter(e => e.classId === classId);
    const tbody = document.getElementById('studentListTableBody');
    tbody.innerHTML = '';

    classStudents.forEach(enrollment => {
        const student = students.find(s => s.id === enrollment.studentId);

        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">${student.studentId}</td>
                        <td class="py-4 px-4 text-gray-700">${student.name}</td>
                        <td class="py-4 px-4 text-gray-700">${student.faculty}</td>
                        <td class="py-4 px-4">
                            <button onclick="removeStudentFromClass(${student.id}, ${classId})" class="text-red-600 hover:text-red-800 font-medium">
                                <i class="fas fa-user-minus mr-1"></i>Remove
                            </button>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });

    document.getElementById('studentListTable').classList.remove('hidden');
}

function editGrade(studentId, classId) {
    const student = students.find(s => s.id === studentId);
    const grade = grades.find(g => g.studentId === studentId && g.classId === classId) ||
        {studentId: studentId, classId: classId, midterm: null, final: null};

    currentEditingGrade = {studentId, classId};

    document.getElementById('gradeStudentName').value = student.name;
    document.getElementById('midtermGrade').value = grade.midterm || '';
    document.getElementById('finalGrade').value = grade.final || '';

    showModal('gradeModal');
}

function removeStudentFromClass(studentId, classId) {
    if (confirm('Are you sure you want to remove this student from the class?')) {
        enrollments = enrollments.filter(e => !(e.studentId === studentId && e.classId === classId));
        grades = grades.filter(g => !(g.studentId === studentId && g.classId === classId));
        loadStudentsList();
        updateStats();
        showNotification('Student removed from class successfully!', 'success');
    }
}

// Modal functions
function showModal(modalId) {
    document.getElementById(modalId).classList.remove('hidden');
}

function hideModal(modalId) {
    document.getElementById(modalId).classList.add('hidden');
}

// Form handlers
document.getElementById('gradeForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const midterm = parseFloat(document.getElementById('midtermGrade').value) || null;
    const final = parseFloat(document.getElementById('finalGrade').value) || null;

    // Validate grades
    if ((midterm !== null && (midterm < 0 || midterm > 20)) ||
        (final !== null && (final < 0 || final > 20))) {
        showNotification('Grades must be between 0 and 20!', 'error');
        return;
    }

    const existingGradeIndex = grades.findIndex(g =>
        g.studentId === currentEditingGrade.studentId &&
        g.classId === currentEditingGrade.classId
    );

    const gradeData = {
        studentId: currentEditingGrade.studentId,
        classId: currentEditingGrade.classId,
        midterm: midterm,
        final: final
    };

    if (existingGradeIndex >= 0) {
        grades[existingGradeIndex] = gradeData;
    } else {
        grades.push(gradeData);
    }

    loadStudentsForGrading();
    updateStats();
    hideModal('gradeModal');
    this.reset();
    showNotification('Grade updated successfully!', 'success');
});

// Notification system
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 z-50 p-4 rounded-xl shadow-lg animate-bounce-in ${
        type === 'success' ? 'bg-green-500' :
            type === 'error' ? 'bg-red-500' : 'bg-blue-500'
    } text-white`;
    notification.innerHTML = `
                <div class="flex items-center">
                    <i class="fas fa-${type === 'success' ? 'check' : type === 'error' ? 'times' : 'info'} mr-2"></i>
                    ${message}
                </div>
            `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// Sample data
const currentStudentId = 1; // John Doe

let courses = [
    {id: 1, code: 'CS101', name: 'Introduction to Programming', credits: 3},
    {id: 2, code: 'CS201', name: 'Data Structures', credits: 3},
    {id: 3, code: 'MATH101', name: 'Calculus I', credits: 4},
    {id: 4, code: 'CS301', name: 'Algorithms', credits: 3}
];

let classes = [
    {id: 1, courseId: 1, professorId: 1, day: 'Monday', startTime: '08:00', endTime: '10:00', location: 'Room 101'},
    {id: 2, courseId: 2, professorId: 1, day: 'Wednesday', startTime: '10:00', endTime: '12:00', location: 'Lab 201'},
    {id: 3, courseId: 3, professorId: 2, day: 'Tuesday', startTime: '14:00', endTime: '16:00', location: 'Room 301'},
    {id: 4, courseId: 4, professorId: 1, day: 'Friday', startTime: '10:00', endTime: '12:00', location: 'Room 201'}
];

let professors = [
    {id: 1, name: 'Dr. Smith'},
    {id: 2, name: 'Dr. Johnson'}
];

let enrollments = [
    {studentId: 1, classId: 1},
    {studentId: 1, classId: 2},
    {studentId: 1, classId: 3}
];

let grades = [
    {studentId: 1, classId: 1, midterm: 18.5, final: 19.0},
    {studentId: 1, classId: 2, midterm: 16.0, final: 17.5},
    {studentId: 1, classId: 3, midterm: 19.0, final: 18.5}
];

// Initialize
document.addEventListener('DOMContentLoaded', function () {
    updateStats();
});

function updateStats() {
    const myEnrollments = enrollments.filter(e => e.studentId === currentStudentId);
    const myGrades = grades.filter(g => g.studentId === currentStudentId);

    // Calculate GPA
    let totalPoints = 0;
    let totalCredits = 0;
    let passedCourses = 0;

    myGrades.forEach(grade => {
        if (grade.midterm !== null && grade.final !== null) {
            const totalGrade = (grade.midterm * 0.4) + (grade.final * 0.6);
            const classInfo = classes.find(c => c.id === grade.classId);
            const courseInfo = courses.find(c => c.id === classInfo.courseId);

            if (totalGrade >= 10) {
                passedCourses++;
                totalCredits += courseInfo.credits;
                totalPoints += (totalGrade / 5) * courseInfo.credits; // Convert to 4.0 scale
            }
        }
    });

    const gpa = totalCredits > 0 ? (totalPoints / totalCredits).toFixed(2) : '0.00';

    document.getElementById('enrolledCourses').textContent = myEnrollments.length;
    document.getElementById('currentGPA').textContent = gpa;
    document.getElementById('creditsEarned').textContent = totalCredits;

    // Update profile stats
    document.getElementById('profileGPA').textContent = gpa;
    document.getElementById('profileCredits').textContent = totalCredits;
    document.getElementById('coursesPassed').textContent = passedCourses;

    // Academic standing
    const gpaNum = parseFloat(gpa);
    let standing = 'Poor';
    if (gpaNum >= 3.5) standing = 'Excellent';
    else if (gpaNum >= 3.0) standing = 'Good';
    else if (gpaNum >= 2.5) standing = 'Satisfactory';
    else if (gpaNum >= 2.0) standing = 'Fair';

    document.getElementById('academicStanding').textContent = standing;
}

// Navigation functions
function showSection(sectionId) {
    document.getElementById('dashboardView').classList.add('hidden');
    document.getElementById('contentArea').classList.remove('hidden');

    // Hide all sections
    const sections = document.querySelectorAll('#contentArea > div:not(:first-child)');
    sections.forEach(section => section.classList.add('hidden'));

    // Show selected section
    document.getElementById(sectionId).classList.remove('hidden');

    // Load section data
    loadSectionData(sectionId);
}

function goBack() {
    document.getElementById('dashboardView').classList.remove('hidden');
    document.getElementById('contentArea').classList.add('hidden');
}

function loadSectionData(sectionId) {
    switch (sectionId) {
        case 'myCourses':
            loadMyCourses();
            break;
        case 'myGrades':
            loadMyGrades();
            break;
    }
}

function loadMyCourses() {
    const myEnrollments = enrollments.filter(e => e.studentId === currentStudentId);
    const container = document.getElementById('courseCards');
    container.innerHTML = '';

    myEnrollments.forEach(enrollment => {
        const classInfo = classes.find(c => c.id === enrollment.classId);
        const courseInfo = courses.find(c => c.id === classInfo.courseId);
        const professorInfo = professors.find(p => p.id === classInfo.professorId);

        const card = `
                    <div class="bg-white rounded-xl p-6 shadow-md border border-gray-100 card-hover">
                        <div class="flex items-center justify-between mb-4">
                            <div class="w-12 h-12 bg-gradient-to-r from-purple-500 to-pink-500 rounded-lg flex items-center justify-center">
                                <i class="fas fa-book text-white"></i>
                            </div>
                            <span class="text-sm font-medium text-gray-500">${courseInfo.credits} credits</span>
                        </div>
                        <h4 class="text-lg font-bold text-gray-800 mb-2">${courseInfo.code}</h4>
                        <p class="text-gray-600 mb-3">${courseInfo.name}</p>
                        <div class="space-y-2 text-sm text-gray-500">
                            <div class="flex items-center">
                                <i class="fas fa-chalkboard-teacher mr-2"></i>
                                ${professorInfo.name}
                            </div>
                            <div class="flex items-center">
                                <i class="fas fa-calendar mr-2"></i>
                                ${classInfo.day}
                            </div>
                            <div class="flex items-center">
                                <i class="fas fa-clock mr-2"></i>
                                ${classInfo.startTime} - ${classInfo.endTime}
                            </div>
                            <div class="flex items-center">
                                <i class="fas fa-map-marker-alt mr-2"></i>
                                ${classInfo.location}
                            </div>
                        </div>
                        <div class="mt-4 pt-4 border-t border-gray-100">
                            <button onclick="dropCourse(${enrollment.classId})" class="text-red-600 hover:text-red-800 font-medium text-sm">
                                <i class="fas fa-times mr-1"></i>Drop Course
                            </button>
                        </div>
                    </div>
                `;
        container.innerHTML += card;
    });
}

function loadMyGrades() {
    const myGrades = grades.filter(g => g.studentId === currentStudentId);
    const tbody = document.getElementById('gradesTable');
    tbody.innerHTML = '';

    let totalPoints = 0;
    let totalCredits = 0;

    myGrades.forEach(grade => {
        const classInfo = classes.find(c => c.id === grade.classId);
        const courseInfo = courses.find(c => c.id === classInfo.courseId);

        const totalGrade = (grade.midterm !== null && grade.final !== null) ?
            ((grade.midterm * 0.4) + (grade.final * 0.6)).toFixed(1) : '-';

        let status = 'In Progress';
        let statusClass = 'text-yellow-600';

        if (totalGrade !== '-') {
            const gradeNum = parseFloat(totalGrade);
            if (gradeNum >= 10) {
                status = 'Passed';
                statusClass = 'text-green-600';
                totalCredits += courseInfo.credits;
                totalPoints += (gradeNum / 5) * courseInfo.credits;
            } else {
                status = 'Failed';
                statusClass = 'text-red-600';
            }
        }

        const row = `
                    <tr class="hover:bg-gray-50 transition-colors duration-150">
                        <td class="py-4 px-4 font-medium text-gray-900">${courseInfo.code}</td>
                        <td class="py-4 px-4 text-gray-700">${courseInfo.name}</td>
                        <td class="py-4 px-4 text-gray-700">${courseInfo.credits}</td>
                        <td class="py-4 px-4 text-gray-700">${grade.midterm || '-'}</td>
                        <td class="py-4 px-4 text-gray-700">${grade.final || '-'}</td>
                        <td class="py-4 px-4 font-medium text-gray-900">${totalGrade}</td>
                        <td class="py-4 px-4">
                            <span class="px-2 py-1 rounded-full text-xs font-medium ${statusClass} bg-current bg-opacity-10">
                                ${status}
                            </span>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });

    // Update overall GPA
    const overallGPA = totalCredits > 0 ? (totalPoints / totalCredits).toFixed(2) : '0.00';
    document.getElementById('overallGPA').textContent = overallGPA;
}

function dropCourse(classId) {
    if (confirm('Are you sure you want to drop this course?')) {
        enrollments = enrollments.filter(e => !(e.studentId === currentStudentId && e.classId === classId));
        grades = grades.filter(g => !(g.studentId === currentStudentId && g.classId === classId));
        loadMyCourses();
        updateStats();
        showNotification('Course dropped successfully!', 'success');
    }
}

// Notification system
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 z-50 p-4 rounded-xl shadow-lg animate-bounce-in ${
        type === 'success' ? 'bg-green-500' :
            type === 'error' ? 'bg-red-500' : 'bg-blue-500'
    } text-white`;
    notification.innerHTML = `
                <div class="flex items-center">
                    <i class="fas fa-${type === 'success' ? 'check' : type === 'error' ? 'times' : 'info'} mr-2"></i>
                    ${message}
                </div>
            `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

let faculties = [
    {
        id: 1,
        name: 'Faculty of Computer Science',
        dean: 'Dr. Sarah Johnson',
        departments: 5,
        established: 1995,
        students: 1250,
        status: 'Active',
        description: 'Leading faculty in computer science and technology education'
    },
    {
        id: 2,
        name: 'Faculty of Engineering',
        dean: 'Dr. Michael Chen',
        departments: 8,
        established: 1987,
        students: 2100,
        status: 'Active',
        description: 'Comprehensive engineering programs with state-of-the-art facilities'
    },
    {
        id: 3,
        name: 'Faculty of Medicine',
        dean: 'Dr. Emily Rodriguez',
        departments: 12,
        established: 1965,
        students: 890,
        status: 'Active',
        description: 'Premier medical education with affiliated teaching hospitals'
    },
    {
        id: 4,
        name: 'Faculty of Business Administration',
        dean: 'Dr. Robert Wilson',
        departments: 6,
        established: 1978,
        students: 1680,
        status: 'Active',
        description: 'Innovative business programs with industry partnerships'
    },
    {
        id: 5,
        name: 'Faculty of Arts and Humanities',
        dean: 'Dr. Lisa Thompson',
        departments: 10,
        established: 1972,
        students: 950,
        status: 'Active',
        description: 'Rich tradition in liberal arts and humanities education'
    },
    {
        id: 6,
        name: 'Faculty of Natural Sciences',
        dean: 'Dr. David Kim',
        departments: 7,
        established: 1983,
        students: 1150,
        status: 'Inactive',
        description: 'Research-focused programs in natural and physical sciences'
    }
];

let filteredFaculties = [...faculties];
let currentPage = 1;
const itemsPerPage = 10;
let deleteTargetId = null;

// Initialize
document.addEventListener('DOMContentLoaded', function () {
    updateStats();
    renderTable();
    updatePagination();
});

function updateStats() {
    const totalFaculties = faculties.length;
    const totalDepartments = faculties.reduce((sum, faculty) => sum + faculty.departments, 0);
    const totalPrograms = totalDepartments * 2; // Assuming 2 programs per department
    const totalMembers = faculties.reduce((sum, faculty) => sum + faculty.students, 0);

    document.getElementById('totalFaculties').textContent = totalFaculties;
    document.getElementById('totalDepartments').textContent = totalDepartments;
    document.getElementById('totalPrograms').textContent = totalPrograms;
    document.getElementById('totalMembers').textContent = totalMembers.toLocaleString();
}

function renderTable() {
    const tbody = document.getElementById('facultyTableBody');
    const emptyState = document.getElementById('emptyState');

    if (filteredFaculties.length === 0) {
        tbody.innerHTML = '';
        emptyState.classList.remove('hidden');
        return;
    }

    emptyState.classList.add('hidden');

    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const pageItems = filteredFaculties.slice(startIndex, endIndex);

    tbody.innerHTML = '';

    pageItems.forEach((faculty, index) => {
        const statusColor = faculty.status === 'Active' ? 'text-green-600 bg-green-100' : 'text-red-600 bg-red-100';

        const row = `
                    <tr class="table-row-hover">
                        <td class="py-6 px-6">
                            <div class="flex items-center">
                                <div class="w-12 h-12 bg-gradient-to-r from-indigo-500 to-purple-500 rounded-xl flex items-center justify-center mr-4">
                                    <i class="fas fa-university text-white"></i>
                                </div>
                                <div>
                                    <div class="text-lg font-bold text-gray-900">${faculty.name}</div>
                                    <div class="text-sm text-gray-500">${faculty.description}</div>
                                </div>
                            </div>
                        </td>
                        <td class="py-6 px-6">
                            <div class="flex items-center">
                                <div class="w-10 h-10 bg-gray-200 rounded-full flex items-center justify-center mr-3">
                                    <i class="fas fa-user-tie text-gray-600"></i>
                                </div>
                                <div>
                                    <div class="font-semibold text-gray-900">${faculty.dean}</div>
                                    <div class="text-sm text-gray-500">Dean</div>
                                </div>
                            </div>
                        </td>
                        <td class="py-6 px-6">
                            <div class="flex items-center">
                                <span class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-teal-100 text-teal-800">
                                    <i class="fas fa-building mr-2"></i>
                                    ${faculty.departments} Departments
                                </span>
                            </div>
                        </td>
                        <td class="py-6 px-6">
                            <div class="text-gray-900 font-medium">${faculty.established}</div>
                            <div class="text-sm text-gray-500">${new Date().getFullYear() - faculty.established} years ago</div>
                        </td>
                        <td class="py-6 px-6">
                            <div class="flex items-center">
                                <span class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-blue-100 text-blue-800">
                                    <i class="fas fa-users mr-2"></i>
                                    ${faculty.students.toLocaleString()}
                                </span>
                            </div>
                        </td>
                        <td class="py-6 px-6">
                            <span class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${statusColor}">
                                <i class="fas fa-circle mr-2 text-xs"></i>
                                ${faculty.status}
                            </span>
                        </td>
                        <td class="py-6 px-6">
                            <div class="flex items-center justify-center space-x-3">
                                <button onclick="viewFaculty(${faculty.id})" class="p-2 text-blue-600 hover:text-blue-800 hover:bg-blue-50 rounded-lg transition-all duration-200" title="View Details">
                                    <i class="fas fa-eye"></i>
                                </button>
                                <button onclick="editFaculty(${faculty.id})" class="p-2 text-green-600 hover:text-green-800 hover:bg-green-50 rounded-lg transition-all duration-200" title="Edit Faculty">
                                    <i class="fas fa-edit"></i>
                                </button>
                                <button onclick="deleteFaculty(${faculty.id})" class="p-2 text-red-600 hover:text-red-800 hover:bg-red-50 rounded-lg transition-all duration-200" title="Delete Faculty">
                                    <i class="fas fa-trash"></i>
                                </button>
                            </div>
                        </td>
                    </tr>
                `;
        tbody.innerHTML += row;
    });
}

function filterFaculties() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const statusFilter = document.getElementById('statusFilter').value;

    filteredFaculties = faculties.filter(faculty => {
        const matchesSearch = faculty.name.toLowerCase().includes(searchTerm) ||
            faculty.dean.toLowerCase().includes(searchTerm) ||
            faculty.description.toLowerCase().includes(searchTerm);
        const matchesStatus = !statusFilter || faculty.status === statusFilter;

        return matchesSearch && matchesStatus;
    });

    currentPage = 1;
    renderTable();
    updatePagination();
}

function sortFaculties() {
    const sortBy = document.getElementById('sortBy').value;

    filteredFaculties.sort((a, b) => {
        switch (sortBy) {
            case 'name':
                return a.name.localeCompare(b.name);
            case 'established':
                return b.established - a.established;
            case 'departments':
                return b.departments - a.departments;
            default:
                return 0;
        }
    });

    renderTable();
}

function updatePagination() {
    const totalPages = Math.ceil(filteredFaculties.length / itemsPerPage);
    const pagination = document.getElementById('pagination');

    // Update showing text
    const startIndex = (currentPage - 1) * itemsPerPage + 1;
    const endIndex = Math.min(currentPage * itemsPerPage, filteredFaculties.length);

    document.getElementById('showingStart').textContent = filteredFaculties.length > 0 ? startIndex : 0;
    document.getElementById('showingEnd').textContent = endIndex;
    document.getElementById('totalRecords').textContent = filteredFaculties.length;

    // Generate pagination buttons
    pagination.innerHTML = '';

    if (totalPages <= 1) return;

    // Previous button
    const prevBtn = `
                <button onclick="changePage(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''} 
                        class="px-3 py-2 rounded-lg border border-gray-200 text-gray-600 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200">
                    <i class="fas fa-chevron-left"></i>
                </button>
            `;
    pagination.innerHTML += prevBtn;

    // Page numbers
    for (let i = 1; i <= totalPages; i++) {
        if (i === 1 || i === totalPages || (i >= currentPage - 1 && i <= currentPage + 1)) {
            const pageBtn = `
                        <button onclick="changePage(${i})" 
                                class="px-4 py-2 rounded-lg border ${i === currentPage ? 'bg-indigo-500 text-white border-indigo-500' : 'border-gray-200 text-gray-600 hover:bg-gray-50'} transition-all duration-200">
                            ${i}
                        </button>
                    `;
            pagination.innerHTML += pageBtn;
        } else if (i === currentPage - 2 || i === currentPage + 2) {
            pagination.innerHTML += '<span class="px-2 py-2 text-gray-400">...</span>';
        }
    }

    // Next button
    const nextBtn = `
                <button onclick="changePage(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''} 
                        class="px-3 py-2 rounded-lg border border-gray-200 text-gray-600 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200">
                    <i class="fas fa-chevron-right"></i>
                </button>
            `;
    pagination.innerHTML += nextBtn;
}

function changePage(page) {
    const totalPages = Math.ceil(filteredFaculties.length / itemsPerPage);
    if (page >= 1 && page <= totalPages) {
        currentPage = page;
        renderTable();
        updatePagination();
    }
}

function viewFaculty(id) {
    const faculty = faculties.find(f => f.id === id);
    if (faculty) {
        alert(`Faculty Details:\n\nName: ${faculty.name}\nDean: ${faculty.dean}\nDepartments: ${faculty.departments}\nEstablished: ${faculty.established}\nStudents: ${faculty.students}\nStatus: ${faculty.status}\n\nDescription: ${faculty.description}`);
    }
}

function editFaculty(id) {
    // Redirect to edit page with faculty ID
    window.location.href = `add-faculty.html?edit=${id}`;
}

function deleteFaculty(id) {
    const faculty = faculties.find(f => f.id === id);
    if (faculty) {
        deleteTargetId = id;
        document.getElementById('deleteItemName').textContent = faculty.name;
        showModal('deleteModal');
    }
}

function confirmDelete() {
    if (deleteTargetId) {
        faculties = faculties.filter(f => f.id !== deleteTargetId);
        filteredFaculties = filteredFaculties.filter(f => f.id !== deleteTargetId);

        // Adjust current page if necessary
        const totalPages = Math.ceil(filteredFaculties.length / itemsPerPage);
        if (currentPage > totalPages && totalPages > 0) {
            currentPage = totalPages;
        }

        updateStats();
        renderTable();
        updatePagination();
        hideModal('deleteModal');
        showNotification('Faculty deleted successfully!', 'success');
        deleteTargetId = null;
    }
}

function exportFaculties() {
    const csvContent = "data:text/csv;charset=utf-8," +
        "Faculty Name,Dean,Departments,Established,Students,Status,Description\n" +
        faculties.map(f => `"${f.name}","${f.dean}",${f.departments},${f.established},${f.students},"${f.status}","${f.description}"`).join("\n");

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "faculties.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    showNotification('Faculty data exported successfully!', 'success');
}

// Modal functions
function showModal(modalId) {
    document.getElementById(modalId).classList.remove('hidden');
}

function hideModal(modalId) {
    document.getElementById(modalId).classList.add('hidden');
}

// Notification system
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 z-50 p-4 rounded-xl shadow-lg animate-bounce-in ${
        type === 'success' ? 'bg-green-500' :
            type === 'error' ? 'bg-red-500' : 'bg-blue-500'
    } text-white`;
    notification.innerHTML = `
                <div class="flex items-center">
                    <i class="fas fa-${type === 'success' ? 'check' : type === 'error' ? 'times' : 'info'} mr-2"></i>
                    ${message}
                </div>
            `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

function showNotifications(message, isSuccess) {
    const icon = isSuccess ? 'fa-check-circle' : 'fa-exclamation-circle';
    const notification = `
        <div class="alert alert-${isSuccess ? 'success' : 'danger'}" 
             style="min-width: 300px; direction: rtl;">
            <i class="fas ${icon} notification-icon"></i>
            ${message}
        </div>
    `;

    $('#notification').html(notification).fadeIn();
    setTimeout(() => $('#notification').fadeOut(), 5000);
}
function hideNotifications() {
    document.getElementById('notification').style.display = 'none';
}
tailwind.config = {
    theme: {
        extend: {
            animation: {
                'slide-in-left': 'slideInLeft 0.6s cubic-bezier(0.68, -0.55, 0.265, 1.55)',
                'slide-out-left': 'slideOutLeft 0.4s ease-in-out',
                'fade-in': 'fadeIn 0.5s ease-out',
                'fade-out': 'fadeOut 0.3s ease-in',
                'progress': 'progress 4s linear',
                'bounce-in': 'bounceIn 0.6s cubic-bezier(0.68, -0.55, 0.265, 1.55)',
            },
            keyframes: {
                slideInLeft: {
                    '0%': {
                        transform: 'translateX(-100%) translateY(-10px)',
                        opacity: '0'
                    },
                    '60%': {
                        transform: 'translateX(10px) translateY(-5px)',
                        opacity: '0.8'
                    },
                    '100%': {
                        transform: 'translateX(0) translateY(0)',
                        opacity: '1'
                    }
                },
                slideOutLeft: {
                    '0%': {
                        transform: 'translateX(0) translateY(0)',
                        opacity: '1'
                    },
                    '100%': {
                        transform: 'translateX(-100%) translateY(-10px)',
                        opacity: '0'
                    }
                },
                fadeIn: {
                    '0%': { opacity: '0' },
                    '100%': { opacity: '1' }
                },
                fadeOut: {
                    '0%': { opacity: '1' },
                    '100%': { opacity: '0' }
                },
                progress: {
                    '0%': { width: '100%' },
                    '100%': { width: '0%' }
                },
                bounceIn: {
                    '0%': {
                        transform: 'scale(0.3) translateX(-100%)',
                        opacity: '0'
                    },
                    '50%': {
                        transform: 'scale(1.05) translateX(5px)',
                        opacity: '0.8'
                    },
                    '70%': {
                        transform: 'scale(0.9) translateX(-2px)',
                        opacity: '0.9'
                    },
                    '100%': {
                        transform: 'scale(1) translateX(0)',
                        opacity: '1'
                    }
                }
            }
        }
    }
}