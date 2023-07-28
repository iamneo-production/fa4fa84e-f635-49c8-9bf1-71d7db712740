import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth:AuthService,private router:Router,private notify:ToastrService){}
  canActivate():boolean{
      if(this.auth.isLoggedIn()){
        return true;
      }else{
        this.notify.error('Please Login First','ERROR!!!',{timeOut:3000})
        this.router.navigate(['login']);
        return false;
      }
  }
  
}