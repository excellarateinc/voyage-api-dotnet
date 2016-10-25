module.exports = function(config) {
  config.set({

    basePath: '',

    files: [
      'node_modules/angular/angular.js', 
      'node_modules/angular-ui-router/release/angular-ui-router.js',
      'node_modules/angular-mocks/angular-mocks.js', 
      'app/app.module.js', 
      'app/app.config.js',
      'app/**/*.js'
    ],

    autoWatch: true,

    singleRun: true,

    frameworks: ['jasmine', 'sinon'],

    browsers: ['PhantomJS'],

    plugins: [
      'karma-chrome-launcher',
      'karma-phantomjs-launcher',
      'karma-jasmine',
      'karma-sinon'
    ],

    logLevel: config.LOG_INFO,
  });
};