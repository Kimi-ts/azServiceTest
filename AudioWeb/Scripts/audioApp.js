﻿var app = angular.module('module1', ['ngAudio']);

app.controller("controller1", ['$scope', 'ngAudio', 'AudioData', function ($scope, ngAudio, audioData) {
    var self = this;

    $scope.isRepeat = false;
    $scope.isShowAddPanel = false;

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
                    title: val.Title,
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
                        if ($scope.isRepeat) {
                            $scope.play(audio);
                        }
                        else {
                            $scope.playNext(val, true);
                        }
                        incPlays(val.name);
                    }
                    return false;
                });
            });
        }
    });

    //$scope.reset = function (audio) {
    //    audio.pause();
    //    audio.setProgress(0);
    //};

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
        //save volume state
        audio.audio.setVolume($scope.currentAudio.audio.volume);

        $scope.currentAudio = audio;
        $scope.play($scope.currentAudio.audio);
    };

    $scope.playNext = function (audio, isAuto) {
        if (audio.playlistIndex < $scope.audios.length - 1) {
            $scope.playSong($scope.audios[audio.playlistIndex + 1]);
            if (!isAuto) {
                incSkips(audio.name);
            };
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

    $scope.uploadedFile = function (element) {
        console.log("uploaded file function called in controller")
        $scope.$apply(function ($scope) {
            $scope.files = element.files;
            console.log($scope.files)
        });
    };

    $scope.addFile = function () {
        var title = $scope.newTitle;
        var artist = $scope.newArtist;
        audioData.postFile($scope.files, {
                artist: artist, title: title
            },
            function (msg) {
                console.log('uploaded');
                $scope.newTitle = "";
                $scope.newArtist = "";
                $scope.files = [];
                $scope.isShowAddPanel = false;
            },
            function (msg) {
                console.log(msg);
                console.log('error');
                $scope.newTitle = "";
                $scope.newArtist = "";
                $scope.files = [];
                $scope.uploadError = msg;
            });
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