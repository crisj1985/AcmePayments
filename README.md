# AcmePayments
* I Used functional programming throughout the application
* I created an app.config file for all the values that could change so I don't have to rely on changing the code
* The key for find the file is called AbsolutePathFile therefore You could create the file and put the path of your file
* I used the following data for the test:
RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00
CRISTIAN=mo03:00-12:00,TH12:00-14:00,SU15:00-21:00
* For the UnitTest the following nugets must be added:MSTest.TestAdapter, MSTest.TestFramework
* It is a Console Application therefore to run it you only have to press press F5(Start)
* The result is:
  The amount to pay RENE is: 215
  The amount to pay ASTRID is: 85
  The amount to pay CRISTIAN is: 360
