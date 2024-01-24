using Discord;
using Moq;

namespace DiscordBot.Tests
{
    public class CooldownTest
    {
        [Fact]
        public void CooldownManager_CooldownTime()
        {
            var manager = new Mock<CooldownManager>();
            var mockUser1 = new Mock<IUser>();
            var mockUser2 = new Mock<IUser>();

            mockUser1.Setup(m => m.Id).Returns(69);
            mockUser2.Setup(m => m.Id).Returns(42);

            manager.SetupGet(m => m.Cooldown).Returns(new TimeSpan(1, 0, 0));
            Assert.False(manager.Object.IsCooldown(mockUser1.Object, out _));
            Assert.False(manager.Object.IsCooldown(mockUser2.Object, out _));
            Assert.True(manager.Object.IsCooldown(mockUser1.Object, out _));
            Assert.True(manager.Object.IsCooldown(mockUser2.Object, out _));
            
            manager.SetupGet(m => m.Cooldown).Returns(new TimeSpan(0, 0, 0));
            Assert.False(manager.Object.IsCooldown(mockUser1.Object, out _));
            Assert.False(manager.Object.IsCooldown(mockUser2.Object, out _));
        }
    }
}
