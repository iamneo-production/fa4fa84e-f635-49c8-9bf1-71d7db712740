import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Validators,FormGroup, FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/Services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  constructor(private router:Router,private auth:AuthService,private fb:FormBuilder,private notif:ToastrService){}
  loginForm!:FormGroup;
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', Validators.required]
    });
  }

  passType:string='password';
  showPass:boolean=false;
  EmailId:string;
  onPassData(){
    localStorage.setItem(this.EmailId,this.loginForm.get('email')?.value);
  }

  onlogin(){
    if (this.loginForm.valid) {
      this.auth.login(this.loginForm.value).subscribe({
        next: (res) => {
          this.loginForm.reset();
          this.auth.storeToken(res.token);
          console.log(res.token);
          this.notif.success('SUCCESS', res.message, { timeOut: 1000 });
          if (this.auth.getRole() === 'admin') {
            this.router.navigate(['admin/getAllLoans']);
          } else {
            this.router.navigate(['user/addLoan']);
          }
        },
        error: (err) => {
          this.notif.error('Error', 'Incorrect email and password', {timeOut: 1000,});
          alert(err.message);
        },
      });
    
    }
   }

 toggle() {
  if (this.showPass) {
    this.showPass = false;
    this.passType = 'password';
  } else {
    this.showPass = true;
    this.passType = 'text';
  }
}

}