var app = angular.module('module1', ['ngAudio']);

app.controller("controller1", ['$scope', 'ngAudio', 'AudioData', function ($scope, ngAudio, audioData) {
    var self = this;

    audioData.getAll({
    }).then(function (response) {
        console.log(response.data);
        if (response.data) {
            $scope.audios = [];
            $.map(response.data, function (val, i) {
                console.log(val.Src);
                var value = {
                    src: val.Src,
                    audio: ngAudio.load(val.Src),
                    name: val.Name,
                    artist: val.Artist,
                    plays: val.Plays,
                    skips: val.Skips,
                    playlistIndex: i
                };
                $scope.audios.push(value);
            });
            //console.log($scope.audios[0]);
            $scope.currentAudio = $scope.audios[0];

            $.map($scope.audios, function (val, i) {
                var audio = val.audio;
                val.audio.complete(function () {
                    if (audio.paused) {
                        audio.setProgress(0);
                        $scope.playNext(val);
                        incPlays(val.name);
                    }
                    return false;
                });
            });
        }
    });

    $scope.reset = function (audio) {
        audio.pause();
        audio.setProgress(0);
    };

    $scope.play = function (audio) {
        $.map($scope.audios, function (val, i) {
            //force to pause other tracks
            if (audio != val.audio && !val.audio.paused) {
                val.audio.pause();
                val.audio.setProgress(0);
            }
        });
        audio.paused ? audio.play() : audio.pause()
    };

    $scope.playSong = function (audio) {
        $scope.currentAudio = audio;
        $scope.play($scope.currentAudio.audio);
    };

    $scope.playNext = function (audio) {
        if (audio.playlistIndex < $scope.audios.length - 1) {
            $scope.playSong($scope.audios[audio.playlistIndex + 1]);
            incSkips(audio.name);
        };
    };

    $scope.playPrev = function (audio) {
        if (audio.playlistIndex > 0) {
            $scope.playSong($scope.audios[audio.playlistIndex - 1]);
            incSkips(audio.name);
        };
    };

    $scope.refreshData = function () {
        audioData.getAll({
        }).then(function (response) {
            if (response.data) {
                $.map($scope.audios, function (val, i) {
                    $.each(response.data, function (index, item) {
                        if (item.Name == val.name) {
                            if (item.Skips != val.skips) {
                                val.skips = item.Skips;
                            }
                            if (item.Plays != val.plays) {
                                val.plays = item.Plays;
                            }
                            return false;
                        }
                    });
                });
            }
        });
    };

    var incPlays = function (songName) {
        audioData.incPlays(songName);
    };

    var incSkips = function (songName) {
        audioData.incSkips(songName);
    };
}]);

app.filter("timeInMins", function () {
    return function (x) {
        var res = "00:00";
        var seconds = Math.floor(x % 60);
        var mins = Math.floor(x / 60);
        if (seconds < 10) {
            seconds = "0" + seconds;
        }
        res = mins + ":" + seconds;
        return res;
    };
});