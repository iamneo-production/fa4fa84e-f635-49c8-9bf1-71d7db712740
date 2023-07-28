import { Injectable } from '@angular/core';
import {HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private Baseurl:string="https://8080-fbdacabdabafefbfafacdefbeabeacfefaaff.project.examly.io"
  
  constructor(private http:HttpClient,private router:Router) { }
  adminsignup(adminobj:any){
    return this.http.post<any>(`${this.Baseurl}/admin/signup`,adminobj);
  } 

   usersignup(userobj:any){
     return this.http.post<any>(`${this.Baseurl}/user/signup`,userobj);
  }

  login(loginobj:any ){
    return this.http.post<any>(`${this.Baseurl}/login`,loginobj);
  }
 

  storeToken(tokenValue: string){
    localStorage.setItem('token',tokenValue);
  }
  signout(){
    localStorage.clear();
    this.router.navigate(['login']);
  }
  getToken(){
    return localStorage.getItem('token');
  }
  isLoggedIn():boolean{
    return !!localStorage.getItem('token')
  }
  decodedToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken();
  
    if (token) {
      return jwtHelper.decodeToken(token);
    }
  }
  
  getID(){
    const x=this.decodedToken();
    return x['nameid'];
  }
  getRole(){
    const x=this.decodedToken();
    return x['role'];
  }

 getEmail(){
  const x=this.decodedToken();
  return x['EmailId'];
 }
}
