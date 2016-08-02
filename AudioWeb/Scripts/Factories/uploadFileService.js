app.service("uploadFileService", ['$http', function($http){

    return {
        uploadfile: function (files, data, success, error) {

            var fd = new FormData();
            var url = '/api/audio';

            angular.forEach(files, function (file) {
                fd.append('file', file);
            });

            var artist = data.artist;
            var title = data.title;
            fd.append("artist", artist);
            fd.append("title", title);

            $http.post(url, fd, {
                withCredentials: false,
                headers: {
                    'Content-Type': undefined
                },
                transformRequest: angular.identity
            })
            .success(function (data) {
                console.log(data);
            })
            .error(function (data) {
                console.log(data);
            });
        }
    }
}]);