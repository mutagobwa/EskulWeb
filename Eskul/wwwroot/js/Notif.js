// Request permission to show notifications
Notification.requestPermission().then(function (permission) {
    if (permission === 'granted') {
        console.log('Notification permission granted.');
    }
});

// Function to show lesson reminder notifications
function showLessonReminderNotification(subjectName, classId, startTime, endTime) {
    var remainingTime = startTime - new Date();

    if (remainingTime <= 900000 && remainingTime > 0) {
        // Show notification for lesson remaining with 15 minutes to start
        setTimeout(function () {
            var notification = new Notification('Lesson Reminder', {
                body: 'The lesson "' + subjectName + '" starts in 15 minutes at ' + formatDate(startTime) + ' in Class ' + classId
            });
        }, remainingTime);
    }

    if (remainingTime <= 0 && new Date() <= endTime) {
        // Show notification for lesson start
        setTimeout(function () {
            var notification = new Notification('Lesson Reminder', {
                body: 'The lesson "' + subjectName + '" has started at ' + formatDate(startTime) + ' in Class ' + classId
            });
        }, 0);
    }
}

// Function to format date in hh:mm AM/PM format
function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

// Register a service worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/service-worker.js').then(function (registration) {
        console.log('Service Worker registered with scope:', registration.scope);
    }).catch(function (error) {
        console.error('Service Worker registration failed:', error);
    });
}
