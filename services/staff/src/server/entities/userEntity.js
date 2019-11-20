import mongoose from 'mongoose';
import autoIncrement from 'mongoose-auto-increment';

autoIncrement.initialize(mongoose.connection);

const userSchema = new mongoose.Schema({
    firstName: String,
    lastName: String,
    username: String,
    salt: String,
    password: String,
    biography: String,
    chatId: String,
    isAdmin: Boolean
});

userSchema.index({
    id: 1,
    username: 1
}, {
    unique: true
});

userSchema.plugin(autoIncrement.plugin, {
    model: 'User',
    field: 'id'
});

export const User = mongoose.model('User', userSchema);
