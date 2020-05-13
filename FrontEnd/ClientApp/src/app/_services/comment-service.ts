import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PostService } from './post-service';
import { UserService } from './user-service';
import { CurrentUserService } from './currentUser-service';

@Injectable({ providedIn: 'root' })
export class CommentService {

    constructor(private http: HttpClient,
        private userService: UserService,
        private postService: PostService,
        private currentUser: CurrentUserService) {
    }

    createComment(
        message: string,
        userId: number,
        postId: number,
        date: string) {
        this.http.post('api/comments',
            { message, postId: postId, userId },
            { headers: { jwt: this.currentUser.getToken() } }
        ).subscribe(
            res => {
                let comment = {
                    id: <number>res,
                    dateCreated: date,
                    message: message,
                    userUserName: this.currentUser.getUsername(),
                    postId: postId
                }

                this.userService.addComment(comment)
                this.postService.addComment(comment)
                this.currentUser.addComment(comment)
            },
            err => console.error(err)
        )
    }

    deleteComment(id: number, postId: number) {
        this.http.delete('api/comments/' + id, {
            headers: { jwt: this.currentUser.getToken() }
        }).subscribe(res => {
            let comment = this.postService.getPost(postId).comments.find(c => c.id == id)

            this.userService.removeComment(comment)
            this.postService.removeComment(comment)
            this.currentUser.removeComment(comment)
        }, err => console.error(err))
    }
}
