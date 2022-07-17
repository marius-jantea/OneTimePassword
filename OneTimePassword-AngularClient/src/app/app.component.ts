import { HttpClient } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { OneTimePasswordService } from './services/onetimepassword.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  public user: User;

  constructor(private oneTimePasswordService: OneTimePasswordService) {
    this.user = new User("1");
  }

  async onSubmit() {
    await this.oneTimePasswordService.generateOneTimePasswordForUser(this.user.id).then(result => {
      console.log(result);
    }, error => console.error(error));

    await this.oneTimePasswordService.getValidOneTimePassword(this.user.id).then(result => {
      if (result) {
        var expirationTime = new Date(Date.now());
        expirationTime.setSeconds(expirationTime.getSeconds() + result.expirationInSeconds);
        this.user.oneTimePassword = new UserOneTimePassword(result.value, expirationTime);
      }
    }, error => console.error(error));;
  }
}

export class User {
  constructor(
    public id: string,
    public oneTimePassword?: UserOneTimePassword
  ) { }
}

export class UserOneTimePassword {
  public value: string;
  public expirationTime: Date;
  constructor(value: string, expirationTime: Date) {
    this.value = value;
    this.expirationTime = expirationTime;
  }
}
