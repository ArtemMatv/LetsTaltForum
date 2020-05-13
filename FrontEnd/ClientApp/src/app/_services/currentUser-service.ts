import { Injectable } from '@angular/core';
import { AuthenticationService } from './authentification-service';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
    private user: IUser

    constructor(authentificationService: AuthenticationService) {
        authentificationService.currentUser.subscribe(x => this.user = x)
    }

    private save(): void {
        localStorage.setItem('currentUser', JSON.stringify(this.user))
    }

    get(): IUser {
        return this.user
    }

    getUsername(): string {
        return this.user.userName
    }

    getToken(): string {
        return this.user.token
    }

    getRole(): string {
        return this.user.userRole.name
    }

    addPost(post: IPost): void {
        this.user.posts.push(post)
        this.save()
    }

    removePost(post: IPost): void {
        this.user.posts = this.user.posts.filter(p => p.id != post.id)
        this.save()
    }

    addComment(comment: IComment): void {
        this.user.comments.push(comment)
        this.save()
    }

    removeComment(comment: IComment): void {
        this.user.comments = this.user.comments.filter(p => p.id != comment.id)
        this.save()
    }
}
