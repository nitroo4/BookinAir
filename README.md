API DU PROJECT / 

1: package necessaire : 
  - [net10.0]: 
   Package de niveau supérieur                          Demandé   Résolu
   > BCrypt.Net-Next                                    4.1.0     4.1.0 
   > Microsoft.AspNetCore.Authentication.JwtBearer      10.0.5    10.0.5
   > Microsoft.AspNetCore.OpenApi                       10.0.1    10.0.1
   > MongoDB.Driver                                     3.7.0     3.7.0 
   > Swashbuckle.AspNetCore                             10.1.7    10.1.7
 

2: dois etre lance par:  
  - dotnet run ou dotnet watch run 

3: necessite connexion internet pour le login 

4: avant tester : Aller dans Services/OtpServices.cs  
  - trouver methode SendEmail (via cntrl + F)
  - modifier fromEmail et password (en votre emeil pour recevoir le otp et password: activer votre 2AF de votre email: et cree un mot de passe app "MAIL" chrome vous donnera le mot de pass a coller) 
