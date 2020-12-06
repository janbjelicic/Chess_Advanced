function Timer(options) {
    var timer,
    instance = this,
    seconds = options.seconds / 10 || 300,
    gain = options.gain || 0,
    id = options.id || "",
    timerEnd = options.onTimerEnd || function () { };

    function updateStatus() {
        var minutesOnClock = Math.floor(seconds / 60);
        var secondsOnClock = Math.floor(seconds % 60);
        $(id).html(minutesOnClock + ":" + secondsOnClock);
    }

    function decrementTimer() {
        seconds = seconds + gain - 1 / 100;
        if(Math.round(seconds) == Math.floor(seconds))
            updateStatus();
        if (Math.round(seconds) === 0) {
            timerEnd();
            instance.stop();
        }        
    }

    this.start = function () {
        clearInterval(timer);
        timer = 0;
        timer = setInterval(decrementTimer, 10);
    };

    this.stop = function () {
        clearInterval(timer);
    };

    this.checkIfTimeRunOut = function () {
        return seconds === 0;
    };
}