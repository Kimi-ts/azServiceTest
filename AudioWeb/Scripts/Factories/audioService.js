﻿app.factory('AudioData', ['$http', function ($http) {
    return {
        getAll: function (params) {
            return $http.get('/api/audio', {
                params: params
            });
        },

        incPlays: function (params) {
            return $http({
                method: "PUT",
                url: "/api/audio/IncPlays",
                data: JSON.stringify(params),
                headers: { "Content-Type": "application/json", Accept: "application/json" } 
            });
        }
    }
}]);

app.service('AudioService', ['AudioData', function (AudioData) {

}]);

////module1.controller('WidgetController', function (WidgetData) {
////    WidgetData.get({
////        id: '0'
////    }).then(function (response) {
////        //Do what you will with the data.
////    })
////});