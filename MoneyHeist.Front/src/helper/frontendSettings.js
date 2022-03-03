/*global getPublicSettings*/

let dockerSettings = null;
if (typeof getPublicSettings === 'function') dockerSettings = getPublicSettings();

export default {
    IsProduction: process.env.NODE_ENV === 'production',
    ApiUrl: dockerSettings && dockerSettings.ApiUrl ? dockerSettings.ApiUrl : process.env.REACT_APP_API_URL,
};
