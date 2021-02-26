#pragma once

namespace Abstractions {
	/*
	* Use this class to handle bytes array data
	*/
	class Bytes {
	private:
		unsigned char* byteArray;
		unsigned int length;

		void copy(unsigned char* byteArray, const unsigned int length);
	public:
		Bytes();
		Bytes(unsigned char* byteArray, const unsigned int length);

		Bytes(const Bytes&);
		Bytes(Bytes&&) noexcept;
		Bytes operator=(const Bytes&);

		const unsigned char* GetBytes() const;

		const unsigned int GetLength() const;

		~Bytes();
	};
}