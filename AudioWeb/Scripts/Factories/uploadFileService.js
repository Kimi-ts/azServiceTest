app.service("uploadFileService", ['$http', function($http){

    return {
        uploadfile: function (files, success, error) {

            var fd = new FormData();
            var url = '/api/audio';

            angular.forEach(files, function (file) {
                fd.append('file', file);
            });

            //sample data
            var data = {
                name: "kimi",
                type: "raiikkonen"
            };

            fd.append("data", JSON.stringify(data));
            console.log("data=");
            console.log(data);

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