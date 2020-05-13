import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CurrentUserService } from './currentUser-service';
import { UserService } from './user-service';

@Injectable({ providedIn: 'root' })
export class AdminService {

    constructor(private http: HttpClient,
        private currentUser: CurrentUserService,
        private userService: UserService) { }

    silenceUser(userName: string, days: number) {
        let user = this.userService.getUser(userName)
        let date: number
        if (user.silencedTo) {
            date = Date.parse(user.silencedTo)

            if (date > Date.now()) {
                date += days * 86400000
            }
            else {
                date = Date.now() + days * 86400000
            }
        }
        else {
            date = Date.now() + days * 86400000
        }

        let newDate = new Date()
        newDate.setMilliseconds(date)
        user.silencedTo = newDate.toString()

        this.http.put('api/users/' + this.currentUser.getUsername() + '/admin', user,
            {
                headers: {
                    jwt: this.currentUser.getToken()
                }
            }).subscribe(result => {
                console.log(result)
            },
                error => console.error(error))
    }

    banUser(userName: string, days: number) {
        let user = this.userService.getUser(userName)
        let date: number
        if (user.bannedTo) {
            date = Date.parse(user.bannedTo)

            if (date > Date.now()) {
                date += days * 86400000
            }
            else {
                date = Date.now() + days * 86400000
            }
        }
        else {
            date = Date.now() + days * 86400000
        }
        let newDate = new Date()
        newDate.setMilliseconds(date)
        user.bannedTo = newDate.toString()

        this.http.put('api/users/' + this.currentUser.getUsername() + '/admin', user,
            {
                headers: {
                    jwt: this.currentUser.getToken()
                }
            }).subscribe(result => {
                console.log(result)
            }, error => console.error(error))
    }
}
