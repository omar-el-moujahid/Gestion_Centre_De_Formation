// SIDEBAR TOGGLE

let sidebarOpen = false;

const sidebar = document.getElementById('sidebar');






//
const table_Students = document.getElementById('StudentsTable');
const table_Admies = document.getElementById('AdminsTable');
const table_Teachers = document.getElementById('TeachersTable');
const table_Courses = document.getElementById('CoursesTable');
const table_Categories = document.getElementById('CategoriesTable');
const dashbord = document.querySelectorAll('.listDASHBOARD');
function openSidebar() {
    if (!sidebarOpen) {
        sidebar.classList.add('sidebar-responsive');
        sidebarOpen = true;
    }
}

function closeSidebar() {
    if (sidebarOpen) {
        sidebar.classList.remove('sidebar-responsive');
        sidebarOpen = false;
    }
}


// JavaScript for Chart.js configuration

const canvasEl = document.getElementById('cardes_el1').getContext('2d'); // Correct element ID
if (canvasEl) {
    const mainBarChart = new Chart(canvasEl, {
        type: 'bar', // Type of chart
        data: {
            labels: [
                'Jan', 'Feb', 'Mar', 'Apr', 'May',
                'Jun', 'Jul', 'Aug', 'Sep', 'Oct',
                'Nov', 'Dec'
            ], // Labels
            datasets: [
                {
                    label: 'Users',
                    data: [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160], // Data values
                    backgroundColor: 'rgba(80, 70, 229, 0.8)',
                },
                {
                    label: 'New Users',
                    data: [30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140],
                    backgroundColor: 'rgba(243, 244, 247, 0.8)',
                },
            ],
        },
        options: {
            responsive: true,
            plugins: {
                tooltip: {
                    enabled: true, // Enable tooltips
                },
                legend: {
                    display: true, // Show legend
                },
            },
            scales: {
                x: {
                    beginAtZero: true,
                },
                y: {
                    beginAtZero: true,
                },
            },
        },
    });
} else {
    console.error('Canvas element with ID "main-bar-chart" not found.');
}


var ctx = document.getElementById('card-chart-new1').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'], // Modifier selon vos données
        datasets: [{
            label: 'Dataset',
            data: [12, 19, 3, 5, 2, 3], // Remplacez par vos données
            borderColor: 'blue',
            backgroundColor: 'rgba(138, 43, 226, 0.1)', // Couleur de l'ombrage
            fill: true, // Permet d'avoir une zone remplie
            tension: 0.4 // Donne une courbe lisse
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: {
                display: false // Cache la légende si non nécessaire
            }
        },
        scales: {
            x: {
                beginAtZero: true
            },
            y: {
                beginAtZero: true
            }
        }
    }
});


function displiayTableStudent() {
    dashbord.forEach(menu => {
        menu.style.display = 'none';
    });
    table_Admies.style.display = 'none';
    table_Teachers.style.display = 'none';
    table_Courses.style.display = 'none';
    table_Categories.style.display = 'none';


    table_Students.style.display = 'block';
}
function displiayTableAdmines() {
    dashbord.forEach(menu => {
        menu.style.display = 'none';
    });
    table_Students.style.display = 'none';
    table_Teachers.style.display = 'none';
    table_Courses.style.display = 'none';
    table_Categories.style.display = 'none';


    table_Admies.style.display = 'block';
}

function displiayTableTeachers() {
    dashbord.forEach(menu => {
        menu.style.display = 'none';
    });
    table_Students.style.display = 'none';
    table_Admies.style.display = 'none';
    table_Courses.style.display = 'none';
    table_Categories.style.display = 'none';


    table_Teachers.style.display = 'block';
}

function displiayTableCourses() {
    dashbord.forEach(menu => {
        menu.style.display = 'none';
    });
    table_Students.style.display = 'none';
    table_Admies.style.display = 'none';
    table_Teachers.style.display = 'none';
    table_Categories.style.display = 'none';


    table_Courses.style.display = 'block';
}

function displiayTableCategories() {
    dashbord.forEach(menu => {
        menu.style.display = 'none';
    });
    table_Students.style.display = 'none';
    table_Admies.style.display = 'none';
    table_Teachers.style.display = 'none';
    table_Courses.style.display = 'none';


    table_Categories.style.display = 'block';
}



function displiayTableDASHBOARD() {
    table_Students.style.display = 'none';
    table_Admies.style.display = 'none';
    table_Teachers.style.display = 'none';
    table_Courses.style.display = 'none';
    table_Categories.style.display = 'none';

    dashbord.forEach(menu => {
        menu.style.display = 'block';
    });
}





