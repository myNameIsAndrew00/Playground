#pragma once
#include "Bytes.h"

namespace Abstractions {
	class BytesReader {
	public:
		BytesReader(const BytesReference& bytes);
		BytesReader(BytesReader&& bytesReader) noexcept;
		BytesReader(const BytesReader&) = delete;
		BytesReader operator=(const BytesReader&) = delete;

		/// <summary>
		/// Get a bool from reader and move curosr to the next bytes available
		/// </summary>
		/// <returns></returns>
		bool PeekBool();

		/// <summary>
		/// Get a char from reader and move cursor to the next bytes available
		/// </summary>
		/// <returns>Char peeked from buffer</returns>
		char PeekChar();

		/// <summary>
		/// Get a int from reader and move cursor to the next bytes available
		/// </summary>
		/// <returns>Integer peeked from buffer</returns>
		int PeekInt();

		/// <summary>
		/// Get a long from reader and move cursor to the next bytes available
		/// </summary>
		/// <returns>Long peeked from buffer</returns>
		long long PeekLong();

		/// <summary>
		/// Get all remaining bytes from the reader and move the cursor to the end.
		/// </summary>
		/// <returns>Bytes peeked from buffer</returns>
		Bytes PeekBytes();

		/// <summary>	 	
		/// Get length bytes from reader and move cursor to the next bytes available
		/// <summary>
		/// <param name="length">Number of bytes taken</param>
		/// <returns>Bytes peeked from buffer</returns>
		Bytes PeekBytes(unsigned int length);


		/// <summary>
		/// Reset the cursor to the first byte
		/// </summary>
		void ResetCursor();
	private:		
		BytesReference bytes;
		unsigned int cursor;
		unsigned int bytesLength;
		const unsigned char* bufferPointer;

		const unsigned char* peekBytes(unsigned int& length);

		template <typename Type>
		const Type* peekType() throw(int);
	};

	template<typename Type>
	inline const Type* BytesReader::peekType() throw(int)
	{
		unsigned int size = sizeof(Type);
		const unsigned char* bytes = this->peekBytes(size);

		//todo: better exception handling;
		if (size != sizeof(Type)) throw (-1);

		return (reinterpret_cast<const Type*>(bytes));
	}

}