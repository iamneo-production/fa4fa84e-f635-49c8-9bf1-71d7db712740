import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/Services/auth.service';
// import {
//   faEye,
//   faEyeSlash,
//   faExclamationTriangle,
//   faEnvelope,
//   faLock}from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  // faTriangleExclamation = faExclamationTriangle;
  // faEnvelope = faEnvelope;
  // faLock = faLock;
  // faEye = faEye;
  // faEyeSlash = faEyeSlash;
  display:boolean=false;
  showPass: boolean = false;
  showConfirmPass: boolean = false;
  passType: string = 'password';
  confirmPassType: string = 'password';
  SignupForm!: FormGroup;
  
  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router,private notif:ToastrService) {

  }
  ngOnInit(): void {
    this.SignupForm = this.fb.group({
      userRole: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      username: ['', Validators.required],
      mobilenumber: ['', [Validators.required,Validators.pattern(/^[6-9]\d{9}$/)]],
      password:['',[Validators.required,Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$')]],
      confirmpassword: ['', Validators.required]
    }, {
      validator: this.passwordMatchValidator
    })
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
   toggleConfirmPass() {

    if (this.showConfirmPass) {
      this.showConfirmPass = false;
      this.confirmPassType = 'password';
    } else {
      this.showConfirmPass = true;
      this.confirmPassType = 'text';
    }
  }
  onsignup() {

    if (this.SignupForm.valid) {
      const usertype = this.SignupForm.get('userRole')?.value
      console.log(this.SignupForm.value);
      if (usertype === 'admin') {
        this.auth.adminsignup(this.SignupForm.value)
        .subscribe({
          next: (res => {
            alert(res.message)
            this.notif.success('Success',"Admin Registerd Successfully",{timeOut:1000});
            this.SignupForm.reset();
            this.router.navigate(['login']);
          })
          , error: (err => {
            this.notif.error('Error', 'Email or Mobile Number is already registered!!!', { timeOut:3000});
          })
        })
      }
      else {
        this.auth.usersignup(this.SignupForm.value)
          .subscribe({
            next: (res => {
              alert(res.message)
              this.SignupForm.reset();
              this.notif.success('Success', 'Account created Successfully!');
              this.router.navigate(['login']);
            })
            , error: (err => {
              this.notif.error('Error', 'Email or Mobile Number is already registered!!!', { timeOut:3000});
            })
          })
      }


    }
  }
  private passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmpassword')?.value;

    if (password !== confirmPassword) {
      form.get('confirmpassword')?.setErrors({ passwordMismatch: true });
    } else {
      form.get('confirmpassword')?.setErrors(null);
    }
  }

}



